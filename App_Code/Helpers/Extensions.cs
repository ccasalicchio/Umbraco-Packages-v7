using RevistaUFO;
using RevistaUFO.Models;
using RevistaUFO.PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using umbraco.MacroEngines;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace RevistaUFO.Helpers
{
    /// <summary>
    /// Summary description for Extensions
    /// </summary>
    public static class Extensions
    {
        static UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

        public static Guid GetGuid(this IPublishedContent node)
        {
            return ApplicationContext.Current.Services.ContentService.GetById(node.Id).Key;
        }
        public static List<IPublishedContent> GetDescendants(this IPublishedContent node)
        {
            return node.Descendants().ToList();
        }
        public static bool IsVisible(this IPublishedContent node)
        {
            return !node.GetPropertyValue<bool>("umbracoNaviHide");
        }
        public static bool IsHighlight(this IPublishedContent node)
        {
            return node.GetPropertyValue<bool>("Thing_highlight_");
        }
        public static bool IsExpired(this IPublishedContent node)
        {
            return node.GetPropertyValue<DateTime>("Thing_highlight_until_") > DateTime.Now;
        }
        public static bool IsRestricted(this IPublishedContent node)
        {
            return node.GetPropertyValue<bool>("Thing_isRestricted_");
        }
        public static bool IsActive(this IPublishedContent node)
        {
            return node.GetPropertyValue<bool>("Thing_isActive_");
        }
        public static bool WillDisplay(this IPublishedContent node)
        {
            return node.IsActive() && node.IsVisible();
        }
        public static bool WillDisplayHighlight(this IPublishedContent node)
        {
            return node.WillDisplay() && node.IsHighlight() && node.IsExpired();
        }
        public static bool WillDisplayRestricted(this IPublishedContent node)
        {
            return node.WillDisplay() && !node.IsRestricted();
        }
        public static void PopulateUserDetails(this EventRegistrationModel model, EventRegistrationUser user)
        {
            model.Fullname = user.Fullname;
            model.Address = user.Address;
            model.Address2 = user.Address2;
            model.Email1 = user.Email1;
            model.Email2 = user.Email2;
            model.Phone1 = user.HomePhone;
            model.Phone2 = user.CellPhone;
            model.City = user.City;
            model.State = user.State;
            model.Neighborhood = user.Neighborhood;
            model.Postal = user.PostalCode;
            model.Number = user.Number;
            model.Document = user.Document;
        }
        public static void PopulateAddOnList(this EventRegistrationModel model)
        {
            foreach (var id in model.AddOns.Substring(0, model.AddOns.Length - 1).Split(','))
            {
                var content = umbracoHelper.Content(id);
                if (model.AddOnList == null) model.AddOnList = new List<IPublishedContent>();
                model.AddOnList.Add(content);
            }
        }
        public static string PopulateTotal(this EventRegistrationModel model)
        {
            int total = 0;
            foreach (var id in model.AddOns.Substring(0, model.AddOns.Length - 1).Split(','))
            {
                var content = umbracoHelper.Content(id);
                total += content.GetPropertyValue<int>("EventAddOnPrice");
            }
            return total.ToString();
        }
        public static List<string> PopulateAddOnListNames(this EventRegistrationModel model)
        {
            List<string> names = new List<string>();
            if (model.AddOns != null)
                foreach (var id in model.AddOns.Substring(0, model.AddOns.Length - 1).Split(','))
                {
                    var content = umbracoHelper.Content(id);
                    names.Add(content.Name);
                }
            return names;
        }

    }
}
