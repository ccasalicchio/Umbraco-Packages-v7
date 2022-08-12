using RevistaUFO.Helpers;
using RevistaUFO.Models;
using RevistaUFO.PetaPoco;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Umbraco.Core.Persistence;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace RevistaUFO.Controllers
{
    /// <summary>
    /// Summary description for PagSeguroController
    /// </summary>
    public class PagSeguroController : RenderMvcController
    {
        static Database db = null;
        static PagSeguroModel pagseguro = null;
        public PagSeguroController() { }
        public PagSeguroController(Database database = null)
        {
            if (database != null) db = database;
        }
        public EventRegistrationModel PopulateRegistrationModel(string session)
        {
            EventRegistrationModel reg = null;

            var registration = PopulateRegistration(session);
            var registrationUser = PopulateRegistrationUser(registration.Id);

            reg = (EventRegistrationModel)registration;
            reg.PopulateUserDetails(registrationUser);
            reg.PopulateAddOnList();

            return reg;
        }
        public EventRegistration PopulateRegistration(string session)
        {
            return db.SingleOrDefault<EventRegistration>("select * from EventRegistrations where session = @0", session) as EventRegistration;
        }
        public EventRegistrationUser PopulateRegistrationUser(int registrationId)
        {
            return db.SingleOrDefault<EventRegistrationUser>("select * from EventRegistrationUser where RegistrationId = @0", registrationId) as EventRegistrationUser;
        }
        public void PopulatePagSeguroInfo(EventRegistrationModel reg, int id = -1)
        {
            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            var page = id == -1 ? umbracoHelper.Content(9033) : Umbraco.Content(id);
            var evento = umbracoHelper.Content(reg.EventId);

            reg.Receiver = page.GetPropertyValue<string>("pagSeguroReceiver");
            reg.Token = page.GetPropertyValue<string>("pagSeguroToken");
            reg.ReturnUrl = umbracoHelper.Content(page.GetPropertyValue<string>("pagSeguroReturnUrl")).UrlWithDomain() + "?session=" + reg.Session;
            //reg.IpAddress = Request == null ? "" : Request.UserHostAddress;
            //reg.RefCode = evento.GetPropertyValue<string>("EventReferenceCode") + reg.RegistrationNumber;

            pagseguro = new PagSeguroModel();

            pagseguro.Receiver = reg.Receiver;
            pagseguro.Token = reg.Token;
            pagseguro.ReturnUrl = reg.ReturnUrl;

        }
        public ActionResult PagSeguro(RenderModel model)
        {
            //has to keep it here, otherwise it returns null for the database
            db = ApplicationContext.DatabaseContext.Database;

            EventRegistrationModel reg = null;

            if (TempData["registrationInfo"] == null)
                reg = PopulateRegistrationModel(Request.QueryString["session"]);
            else reg = TempData["registrationInfo"] as EventRegistrationModel;

            PopulatePagSeguroInfo(reg, model.Content.Id);

            //Change to 'payment completed' in the DB (update registration)
            reg.PaymentConfirmation = GetPaymentString(reg, API.ContentType.ISO_8859_1, model.Content.GetPropertyValue<bool>("pagSeguroSandbox"));
            TempData["registrationInfo"] = reg;

            return CurrentTemplate(model);
        }
        public string ConfirmPayment(string session, string transaction)
        {
            var registration = PopulateRegistration(session);
            var model = PopulateRegistrationModel(session);
            PopulatePagSeguroInfo(model);
            string referenceCode = model.RefCode;

            if (ConfirmTransation(transaction, referenceCode, true))
            {
                registration.RegistrationDescription = string.Format("Confirmação do pagamento: {0}", transaction);
                registration.PaymentConfirmation = transaction;
                registration.Paid = true;
                db.Update(registration);
                return "Pagamento Concluido com sucesso. obrigado!";
            }
            else
            {
                Debug.Write("pagamento ainda não confirmado");
                return "O pagamento não foi processado corretamente. Favor entrar em contato com suporte@ufo.com.br";
            }
        }

        public bool ConfirmTransation(string transaction, string referenceCodeRegistered, bool isSandbox = false)
        {
            string url = isSandbox ? "https://ws.sandbox.pagseguro.uol.com.br/v3/transactions/{0}" : "https://ws.pagseguro.uol.com.br/v3/transactions/{0}";

            string destination = string.Format(url, transaction);
            string request = string.Format("?email={0}&token={1}", pagseguro.Receiver, pagseguro.Token);
            API api = new API();
            string data = api.postStringData(destination, request, API.ContentType.ISO_8859_1);
            XDocument doc = XDocument.Parse(data);
            int status = int.Parse(doc.Descendants("status").SingleOrDefault().Value);
            string referenceCode = doc.Descendants("reference").SingleOrDefault().Value;
            if (!referenceCode.Equals(referenceCodeRegistered)) return false;
            return (status == 3 || status == 2 || status == 1); //accept status 1 for sandbox
        }
        public string GetPaymentString(EventRegistrationModel model, API.ContentType type, bool isSandbox = false)
        {
            string post = isSandbox ? "https://ws.sandbox.pagseguro.uol.com.br/v2/checkout?email={0}&token={1}" : "https://ws.pagseguro.uol.com.br/v2/checkout?email={0}&token={1}";
            post = string.Format(post, HttpUtility.HtmlEncode(model.Receiver), model.Token);
            string xml = GenerateXML(model);
            API api = new API();
            string returnXml = api.postXMLData(post, xml, type);
            XDocument doc = XDocument.Parse(returnXml);
            return doc.Descendants("code").SingleOrDefault().Value;
        }

        string GenerateXML(EventRegistrationModel model)
        {
            string phoneRegex = @"\(([0-9]+)\)\s?([0-9]+)-([0-9]+)";
            Regex phoneFind = new Regex(phoneRegex, RegexOptions.IgnoreCase);
            Match match = phoneFind.Match(model.Phone1);
            var ddd = match.Groups[1].Value;
            var phone = match.Groups[2].Value + match.Groups[3].Value;
            var trimmedDoc = model.Document.Trim().Replace(".", "").Replace("-", "");

            /*
             * Name: 0
             * Email: 1
             * Area Code: 2
             * Phone: 3
             * IP: 4
             * CPF: 5
             * Items: 6 //array of items
             * redirectUrl: 7
             * Reference Code: 8
             * */
            string rawXml = "<?xml version='1.0'?><checkout><sender><name>{0}</name><email>{1}</email><phone><areaCode>{2}</areaCode><number>{3}</number></phone><ip>{4}</ip><documents><document><type>CPF</type><value>{5}</value></document></documents></sender><currency>BRL</currency><items>{6}</items><redirectURL>{7}</redirectURL><reference>{8}</reference></checkout>";
            string item = "<item><id>{0}</id><description>{1}</description><amount>{2}.00</amount><quantity>{3}</quantity><weight>{4}</weight></item>";
            string items = string.Empty;

            model.PopulateAddOnList();
            foreach (dynamic product in model.AddOnList)
                items += string.Format(item, product.Id, HttpUtility.HtmlEncode(product.Name), product.EventAddOnPrice, 1, 0);

            return string.Format(rawXml,
                             model.Fullname,
                             model.Email1,
                             ddd,
                             phone,
                             model.IpAddress,
                             trimmedDoc,
                             items,
                             model.ReturnUrl,
                             model.RefCode
                             );
        }
    }
}