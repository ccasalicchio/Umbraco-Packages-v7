using CsvHelper;
using Newtonsoft.Json;
using RevistaUFO.Helpers;
using RevistaUFO.Models;
using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for MemberLoginSurfaceController
    /// </summary>
    public class EventRegistrationReportSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private IContentService service = null;
        static Database db = null;
        IPublishedContent home = null;
        Smtp email;
        public EventRegistrationReportSurfaceController()
        {
            home = umbracoHelper.TypedContentAtRoot().First();
            db = ApplicationContext.DatabaseContext.Database;
            service = ApplicationContext.Services.ContentService;
            email = new Smtp();
        }

        [HttpGet]
        public JsonResult GetAvailableAddOns(int id)
        {
            var evento = umbracoHelper.TypedContent(id);
            var addons = new List<AddOn>();
            if (evento.Children.Any(x => x.DocumentTypeAlias == "EventAddOn"))
                foreach (var addon in evento.Children.Where(x => x.DocumentTypeAlias == "EventAddOn"))
                    addons.Add(new AddOn
                    {
                        Name = addon.Name,
                        Currency = addon.GetPropertyValue<string>("EventAddOnCurrency"),
                        Price = addon.GetPropertyValue<double>("EventAddOnPrice"),
                        Description = addon.GetPropertyValue<string>("EventAddOnDescription"),
                        Id = addon.Id
                    });
            return Json(addons, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintAll(int eventId)
        {
            var all = db.Query<EventRegistrationModel>("select a.Id, EventId, EventAddOns as AddOns, Coupon, TotalPaid as Total, Paid, PaymentMethod, RefCode, IpAddress, PaymentConfirmation, RegistrationNumber, [Session], RegistrationDescription, CreatedOn, Fullname, Email1, Email2, HomePhone as Phone1, CellPhone as Phone2, Address, Address2, Number, Neighborhood, City,State,PostalCode as Postal, Document  from EventRegistrations a join EventRegistrationUser b on a.id = b.registrationId where EventId = " + eventId).ToList();

            return View("Views/ImprimirListaDeInscritos", all);

        }
        [HttpGet]
        public JsonResult GetAll(int id)
        {
            var all = db.Query<EventRegistrationModel>("select a.Id, EventId, EventAddOns as AddOns, Coupon, TotalPaid as Total, Paid, PaymentMethod, RefCode, IpAddress, PaymentConfirmation, RegistrationNumber, [Session], RegistrationDescription, CreatedOn, Fullname, Email1, Email2, HomePhone as Phone1, CellPhone as Phone2, Address, Address2, Number, Neighborhood, City,State,PostalCode as Postal, Document  from EventRegistrations a join EventRegistrationUser b on a.id = b.registrationId where EventId = " + id).ToList();

            return Json(all, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public JsonResult Delete([System.Web.Http.FromUri] int[] ids)
        {
            string strIds = string.Join(",", ids);
            db.Execute(string.Format("delete from EventRegistrationUser where registrationId in ({0})", strIds));
            db.Execute(string.Format("delete from EventRegistrations where id in ({0})", strIds));
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public JsonResult DeleteOne(int id)
        {
            return this.Delete(new int[] { id });
        }
        [HttpGet]
        public JsonResult Update(EventRegistrationModel model)
        {
            EventRegistration reg = model;
            reg.RegistrationDescription = model.Paid ? string.Format("Confirmação Manual {0}", model.RegistrationDescription) : model.RegistrationDescription;
            reg.IpAddress = model.IpAddress ?? Request.UserHostAddress;

            EventRegistrationUser user = model;
            db.Update(user);
            db.Update(reg);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        long Latest(int eventId)
        {
            long? scalar = db.ExecuteScalar<long>("select max(RegistrationNumber) from EventRegistrations where eventID = @0", eventId);
            if (scalar.HasValue) return scalar.Value;
            return 1;
        }
        [HttpGet]
        public JsonResult ManualRegistration(EventRegistrationModel registration)
        {
            var evento = umbracoHelper.TypedContent(registration.EventId);
            var refCode = evento.GetPropertyValue<string>("EventReferenceCode") + (Latest(registration.EventId) + 1);

            var newRegistration = new EventRegistration
            {
                EventAddOns = registration.AddOns,
                Coupon = registration.Coupon,
                EventId = registration.EventId,
                PaymentMethod = (int)registration.PaymentMethod,
                TotalPaid = registration.Total,
                Session = Guid.NewGuid().ToString(),
                RegistrationDescription = registration.Paid ? string.Format("Inscrição Manual {0}", registration.RegistrationDescription) : registration.RegistrationDescription,
                CreatedOn = DateTime.Now,
                RefCode = registration.RefCode ?? refCode,
                IpAddress = registration.IpAddress ?? Request.UserHostAddress,
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

            var done = db.Insert(newRegistration);

            //Assign a registration ID
            var registrationNumber = Latest(registration.EventId) + 1;
            newRegistration.RegistrationNumber = registrationNumber;
            int updated = db.Update(newRegistration);

            //Assing foreign key
            registrationUser.RegistrationId = int.Parse(done.ToString());
            var user = db.Insert(registrationUser);

            registration.Session = newRegistration.Session;
            registration.RegistrationNumber = newRegistration.RegistrationNumber;

            return Json(registration, JsonRequestBehavior.AllowGet);
        }


        public FileContentResult ExportToCSV(int eventId)
        {
            var all = db.Query<EventRegistrationModel>(
                "select a.Id, EventId, EventAddOns as AddOns, Coupon, TotalPaid as Total, Paid, PaymentMethod, RefCode, IpAddress, PaymentConfirmation, RegistrationNumber, [Session], RegistrationDescription, CreatedOn, Fullname, Email1, Email2, HomePhone as Phone1, CellPhone as Phone2, Address, Address2, Number, Neighborhood, City,State,PostalCode as Postal, Document  from EventRegistrations a join EventRegistrationUser b on a.id = b.registrationId where EventId = " + eventId);

            var sw = new StringWriter();
            //write the header
            sw.WriteLine("Id;Evento;Inclusos;Cupom;Total;Pago;Tipo de Pagamento;Referencia;IP;Confirmacao;Registro;Descricao;Data;Nome Completo; Email1;Email2;Tel Residencial;Tel Celular;Endereco;Complemento;Numero;Bairro;Cidade;Estado;Cep;CPF");

            foreach (var record in all)
            {
                record.PopulateAddOnList();
                var included = string.Empty;
                foreach (var addon in record.AddOnList) included += addon.Name + "-";
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25}", record.Id, record.EventId, included, record.Coupon, record.Total, record.Paid, record.PaymentMethod, record.RefCode, record.IpAddress, record.PaymentConfirmation, record.RegistrationNumber, record.RegistrationDescription, record.CreatedOn, record.Fullname, record.Email1, record.Email2, record.Phone1, record.Phone2, record.Address, record.Address2, record.Number, record.Neighborhood, record.City, record.State, record.Postal, record.Document));
            }

            var evento = umbracoHelper.TypedContent(eventId);
            return File(new System.Text.UTF8Encoding().GetBytes(sw.ToString().ToCharArray()), "text/csv", evento.Name + ".csv");

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