using Newtonsoft.Json.Linq;
using RevistaUFO.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using umbraco.MacroEngines;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.Security;

namespace RevistaUFO.Helpers
{
    /// <summary>
    /// RevistaUFOHelper is a helper for various funcionalities of the site
    /// </summary>
    public static class Helper
    {
        private static UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);
        private static MembershipHelper members = new MembershipHelper(UmbracoContext.Current);
        public static MemberPublishedContent CurrentMember
        {
            get
            {
                return (MemberPublishedContent)members.GetCurrentMember();
            }
        }
        public static IPublishedContent Root
        {
            get
            {
                return helper.TypedContentAtRoot().First();
            }
        }
        public static string GetUserIP()
        {
            string ipList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        public static string GetScrollId(IPublishedContent node, string documentType)
        {
            dynamic page = GetPage(node, documentType);
            if (page.HasValue("Navigation_Scroll_Id")) return page.GetPropertyValue("Navigation_Scroll_Id");
            else return "";
        }

        public static dynamic GetPage(string documentType)
        {
            dynamic page = Root
            .Descendants(documentType)
            .FirstOrDefault();

            return page;
        }
        public static IPublishedContent GetPublishedPage(string documentType)
        {
            var page = Root.Descendants(documentType).FirstOrDefault();
            return page;
        }
        public static dynamic GetPage(int id)
        {
            dynamic node = Root
            .DescendantsOrSelf().Where(x => x.Id == id).FirstOrDefault();

            return node;
        }

        public static DynamicNode GetPage(IPublishedContent node, string documentType)
        {

            //To be used with this syntax
            /*
            Helper.GetPage(Umbraco.TypedContentAtRoot().First(),"DocumentType");
            */

            dynamic list = node
            .Descendants(documentType)
            .FirstOrDefault();

            return new DynamicNode(list.Id);
        }

        public static IList<DynamicNode> GetDescendants(IPublishedContent node, string parentDocumentType, string documentType)
        {

            //To be used with this syntax
            /*
            Helper.GetDescendants(Umbraco.TypedContentAtRoot().First(),"ParentDocumentType","DocumentType")
            */

            dynamic list = node
            .Descendants(parentDocumentType)
            .FirstOrDefault();

            return new DynamicNode(list.Id).Descendants(documentType).Items;
        }
        public static List<IPublishedContent> GetDescendants(IPublishedContent node)
        {
            return node.Descendants().ToList();
        }
        public static IList<DynamicNode> GetDescendants(DynamicNode node, string documentType)
        {
            return node.Descendants(documentType).ToList();
        }

        public static IList<DynamicNode> GetDescendants(string parentDocumentType, string documentType)
        {
            dynamic list = Root
            .Descendants(parentDocumentType)
            .FirstOrDefault();

            return new DynamicNode(list.Id).Descendants(documentType).Items;
        }

        public static string GetUserName(int userId)
        {
            var userService = ApplicationContext.Current.Services.UserService;
            return userService.GetProfileById(userId).Name;
        }

        public static string GetUserName(string userId)
        {
            int id = int.Parse(userId);
            return GetUserName(id);
        }

        public static IMember GetMemberById(int memberId)
        {
            var memberService = ApplicationContext.Current.Services.MemberService;
            return memberService.GetById(memberId);
        }

        public static IMember GetMemberById(string memberId)
        {
            int id = int.Parse(memberId);
            return GetMemberById(id);
        }
        /// <summary>
        /// from http://stackoverflow.com/questions/1342775/what-is-the-best-way-to-clean-a-url-with-a-title-in-it
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string CleanUrl(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            // replace hyphens to spaces, remove all leading and trailing whitespace
            value = value.Replace("-", " ").Trim().ToLower();

            // replace multiple whitespace to one hyphen
            value = Regex.Replace(value, @"[\s]+", "-");

            // replace umlauts and eszett with their equivalent
            value = value.Replace("ß", "ss");
            value = value.Replace("ä", "ae");
            value = value.Replace("ö", "oe");
            value = value.Replace("ü", "ue");

            //remove unwanted characters from string
            value = value.Replace("?", string.Empty);
            value = value.Replace("!", string.Empty);
            value = value.Replace("@", string.Empty);
            value = value.Replace("#", string.Empty);
            value = value.Replace("$", string.Empty);
            value = value.Replace("%", string.Empty);
            value = value.Replace("&", string.Empty);
            value = value.Replace("*", string.Empty);
            value = value.Replace("(", string.Empty);
            value = value.Replace(")", string.Empty);
            value = value.Replace(".", string.Empty);
            value = value.Replace(",", string.Empty);
            value = value.Replace(";", string.Empty);
            value = value.Replace(":", string.Empty);
            value = value.Replace("'", string.Empty);
            value = value.Replace("~", string.Empty);

            // removes diacritic marks (often called accent marks) from characters
            value = RemoveDiacritics(value);

            // remove all left unwanted chars (white list)
            value = Regex.Replace(value, @"[^a-z0-9\s-]", String.Empty);

            return value;
        }
        public static string RemoveDiacritics(string value)
        {
            if (String.IsNullOrEmpty(value))
                return value;

            string normalized = value.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            Encoding nonunicode = Encoding.GetEncoding(850);
            Encoding unicode = Encoding.Unicode;

            byte[] nonunicodeBytes = Encoding.Convert(unicode, nonunicode, unicode.GetBytes(sb.ToString()));
            char[] nonunicodeChars = new char[nonunicode.GetCharCount(nonunicodeBytes, 0, nonunicodeBytes.Length)];
            nonunicode.GetChars(nonunicodeBytes, 0, nonunicodeBytes.Length, nonunicodeChars, 0);

            return new string(nonunicodeChars);
        }
        public static string GetPreValue(IPublishedContent node, string propertyName)
        {
            return umbraco.library.GetPreValueAsString(node.GetPropertyValue<int>(propertyName));
        }
        public static string GetPreValue(object propertyId)
        {
            return umbraco.library.GetPreValueAsString(int.Parse(propertyId.ToString()));
        }
        public static string StripParagraph(string html)
        {
            if (html.Length > 5)
            {
                html = html.Trim();
                string htmlLower = html.ToLower();

                // the field starts with an opening p tag
                if (htmlLower.Substring(0, 3) == "<p>"
                    // it ends with a closing p tag
                    && htmlLower.Substring(html.Length - 4, 4) == "</p>"
                    // it doesn't contain multiple p-tags
                    && htmlLower.IndexOf("<p>", 1) < 0)
                {
                    html = html.Substring(3, html.Length - 7);
                }
            }
            return html;
        }
        public static string ThemeName(string color)
        {
            /*
             *  @light-red: #E53527;
                @red: #C00418;
                @dark-red: #8B0E13;
                @maroon: #7B2D0F;
                @orange: #EE7F00;
                @gold: #FFC300;
                @light-green: #65AB44;
                @green: #45A12B;
                @dark-green: #466624;
                @blue: #0077B2;
                @purple: #5E1A6F;
                @dark-pink: #BB1580;
             * */

            switch (color.ToUpper())
            {
                case "E53527": return "light-red";
                case "C00418": return "red";
                case "8B0E13": return "dark-red";
                case "7B2D0F": return "maroon";
                case "EE7F00": return "orange";
                case "FFC300": return "gold";
                case "65AB44": return "light-green";
                case "45A12B": return "green";
                case "466624": return "dark-green";
                case "0077B2": return "blue";
                case "5E1A6F": return "purple";
                case "BB1580": return "dark-pink";
                default: return "green";
            }
        }
        public static string GenerateQR(int size = 150, string data = "")
        {
            return string.Format("https://api.qrserver.com/v1/create-qr-code/?size={0}x{0}&data={1}", size, data);
        }
        public static List<RelatedLink> GetRelatedLinks(IPublishedContent contentItem, string propertyAlias)
        {
            List<RelatedLink> relatedLinksList = null;
            UmbracoHelper uHelper = new UmbracoHelper(UmbracoContext.Current);
            IPublishedProperty relatedLinkProperty = contentItem.GetProperty(propertyAlias);

            if (relatedLinkProperty != null && relatedLinkProperty.HasValue && relatedLinkProperty.Value.ToString().Length > 2)
            {
                relatedLinksList = new List<RelatedLink>();
                JArray relatedLinks = (JArray)relatedLinkProperty.Value;

                foreach (JObject linkItem in relatedLinks)
                {
                    string linkUrl = (linkItem.Value<bool>("isInternal")) ? uHelper.NiceUrl(linkItem.Value<int>("internal")) : linkItem.Value<string>("link");
                    bool newWindow = linkItem.Value<bool>("newWindow");
                    string linkText = linkItem.Value<string>("caption");
                    relatedLinksList.Add(new RelatedLink(linkText, linkUrl, newWindow));
                }
            }

            return relatedLinksList;
        }
    }
}

