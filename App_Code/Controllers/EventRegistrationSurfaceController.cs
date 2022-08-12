using RevistaUFO.Helpers;
using RevistaUFO.Models;
using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Web;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for MemberLoginSurfaceController
    /// </summary>
    public class EventRegistrationSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        readonly UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        static Database _db = null;
        readonly IPublishedContent home = null;
        readonly Smtp email;
        public EventRegistrationSurfaceController()
        {
            home = umbracoHelper.TypedContentAtRoot().First();
            _db = ApplicationContext.DatabaseContext.Database;
            email = new Smtp();
        }

        [HttpGet]
        [OutputCache(Duration = 24 * 60 * 60, VaryByParam = "none")]
        public JsonResult AllEvents()
        {
            /*[{ "date": "1337594400000", "type": "meeting", "title": "Project A meeting", "description": "Lorem Ipsum dolor set", "url": "http://www.event1.com/" }]*/

            var events = umbracoHelper.TypedContentAtRoot().First().Descendants("List_Events").FirstOrDefault().Descendants("Event_Main");
            List<EventCalendarItem> eventos = new List<EventCalendarItem>();
            foreach (var e in events)
                eventos.Add(new EventCalendarItem
                {
                    title = e.GetPropertyValue<string>("eventName"),
                    description = e.GetPropertyValue<string>("Event_LongDescription"),
                    url = e.Url,
                    LongDate = e.GetPropertyValue<DateTime>("eventStartDate")
                });
            //Cache.Insert("EventCalendar", eventos, null, DateTime.Now.AddMinutes(1d), Cache.NoSlidingExpiration);
            return Json(eventos, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public ActionResult RegisterForEventPost(EventRegistrationModel form)
        {
            if (ModelState.IsValid)
            {
                var evento = umbracoHelper.Content(form.EventId);
                //cupons not enabled yet
                form.Coupon = "";
                form.IpAddress = Request.UserHostAddress;
                form.RefCode = evento.GetPropertyValue<string>("EventReferenceCode") + (Latest(form.EventId) + 1);

                var done = NewRegister(form);

                if (done != null)
                {
                    string qr = HttpUtility.HtmlEncode(string.Format("nome:{0};inscricao:{1};refCode:{2}", done.Fullname, done.RegistrationNumber, form.RefCode));
                    string payLink = evento.Parent.UrlWithDomain() + done.PaymentMethod.ToString() + "?session=" + done.Session;

                    string message = evento.GetPropertyValue<string>("EventConfirmationEmail")
                        .Replace("##NAME##", done.Fullname)
                        .Replace("##EMAIL##", done.Email1)
                        .Replace("##ID##", qr)
                        .Replace("##PAYLINK##", payLink)
                        .Replace("##DATE##", DateTime.Now.ToShortDateString())
                        .Replace("##EVENTNAME##", evento.Name)
                        .Replace("##IPADDRESS##", form.IpAddress)
                        .Replace("##VIEWURL##", evento.Parent.UrlWithDomain() + "InscricaoConcluida?session=" + done.Session)
                        .Replace("##REFCODE##", form.RefCode);

                    SendRegistrationConfirmation(done.Email1, "Inscrição para o " + evento.Name, message);
                    TempData["registrationInfo"] = done;

                    if (done.PaymentMethod != PaymentMethod.Free)
                    {
                        switch (done.PaymentMethod)
                        {
                            case PaymentMethod.PayPal:
                                break;
                            case PaymentMethod.PagSeguro:
                                return Redirect(evento.Parent.Url + "PagSeguro?session=" + done.Session);
                            case PaymentMethod.Free: //nothing to pay
                                break;
                            default: //nothing to pay
                                break;
                        }
                    }
                }
            }
            else
            {
                TempData["Status"] = "Erro no formulário de inscrição, tente novamente";
                TempData["formdata"] = form;
            }
            return CurrentUmbracoPage();
        }

        bool PayWithPagSeguro()
        {
            return false;
        }
        bool PayWithPayPal()
        {
            return false;
        }
        long Latest(int eventId)
        {
            long? scalar = _db.ExecuteScalar<long>("select max(RegistrationNumber) from EventRegistrations where eventID = @0", eventId);
            if (scalar.HasValue) return scalar.Value;
            return 1;
        }
        EventRegistrationModel NewRegister(EventRegistrationModel registration)
        {
            //get total price from system to avoid price hacking
            string total = registration.PopulateTotal();

            var newRegistration = new EventRegistration
            {
                EventAddOns = registration.AddOns,
                Coupon = registration.Coupon,
                EventId = registration.EventId,
                PaymentMethod = (int)registration.PaymentMethod,
                TotalPaid = total.ToString(CultureInfo.InvariantCulture),
                Session = Guid.NewGuid().ToString(),
                RegistrationDescription = "Pagamento pendente",
                CreatedOn = DateTime.UtcNow,
                IpAddress = registration.IpAddress,
                RefCode = registration.RefCode,
                Paid = registration.Paid
            };

            var registrationUser = new EventRegistrationUser
            {
                Fullname = registration.Fullname,
                Email1 = registration.Email1,
                Email2 = registration.Email2,
                HomePhone = registration.Phone1,
                CellPhone = registration.Phone2,
                PostalCode = registration.Postal,
                Address = registration.Address,
                Address2 = registration.Address2,
                City = registration.City,
                Number = registration.Number,
                Neighborhood = registration.Neighborhood,
                State = registration.State,
                Document = registration.Document
            };

            var done = _db.Insert(newRegistration);

            //Assign a registration ID
            var registrationNumber = Latest(registration.EventId) + 1;
            newRegistration.RegistrationNumber = registrationNumber;
            int updated = _db.Update(newRegistration);

            //Assing foreign key
            registrationUser.RegistrationId = int.Parse(done.ToString());
            var user = _db.Insert(registrationUser);

            registration.Session = newRegistration.Session;
            registration.RegistrationNumber = newRegistration.RegistrationNumber;

            return registration;
        }

        public bool SendRegistrationConfirmation(string emailAddress, string subject, string template)
        {
            var emailMessage = template;
            SetupSmtp();
            email.To = emailAddress;
            email.ToName = emailAddress;
            email.Subject = subject;
            email.Message = email.UsingTemplate(emailMessage,
                new
                {
                    Website = "Site Revista UFO",
                    Email = emailAddress
                });

            return email.Send();
        }

        void SetupSmtp()
        {
            email.Hostname = home.GetPropertyValue<string>("hostname");
            email.Port = home.GetPropertyValue<int>("port");
            email.Username = home.GetPropertyValue<string>("username");
            email.Password = home.GetPropertyValue<string>("password");
            email.UseSSL = home.GetPropertyValue<bool>("useSSL");
            email.From = home.GetPropertyValue<string>("from");
            email.FromName = home.GetPropertyValue<string>("fromName");
        }
    }
}