using System;
using System.IO;
using System.Web;
using System.Xml;

using Umbraco.Tools.ConfigurationActions.Modules;

namespace Umbraco.Tools.ConfigurationActions.Extensions
{
    public static class DashboardExtensions
    {
        public static string DASHBOARD_CONFIG = Umbraco.Core.IO.SystemFiles.DashboardConfig;

        private static XmlDocument GetConfiguration()
        {
            var configPath = HttpContext.Current.Server.MapPath(DASHBOARD_CONFIG);
            var dashboardConfiguration = new XmlDocument();
            dashboardConfiguration.Load(configPath);
            return dashboardConfiguration;
        }

        public static void InstallDashboard(this IDashboard dashboard)
        {
            var xml = GetConfiguration();
            var existingDashboard = xml.SelectSingleNode("//section[@alias='" + dashboard.Alias + "']");
            if (existingDashboard is null)
            {
                var section = xml.CreateElement("section");
                var aliasAttr = xml.CreateAttribute("alias");
                aliasAttr.Value = dashboard.Alias;
                section.Attributes.Append(aliasAttr);

                var access = xml.CreateElement("access");
                foreach (var deny in dashboard.AccessDeny)
                {
                    var denyNode = xml.CreateElement("deny");
                    denyNode.InnerText = deny;
                    access.AppendChild(denyNode);
                }
                foreach (var grant in dashboard.AccessGrant)
                {
                    var grantNode = xml.CreateElement("grant");
                    grantNode.InnerText = grant;
                    access.AppendChild(grantNode);
                }

                var areas = xml.CreateElement("areas");
                foreach (var area in dashboard.Areas)
                {
                    var areaNode = xml.CreateElement("area");
                    areaNode.InnerText = area;
                    areas.AppendChild(areaNode);
                }

                var tab = xml.CreateElement("tab");
                var tabCaptionAttr = xml.CreateAttribute("caption");
                tabCaptionAttr.Value = dashboard.TabCaption;
                tab.Attributes.Append(tabCaptionAttr);

                var control = xml.CreateElement("control");
                control.InnerText = dashboard.Control;
                tab.AppendChild(control);

                section.AppendChild(access);
                section.AppendChild(areas);
                section.AppendChild(tab);

                xml.ChildNodes[1].AppendChild(section);

                var configPath = HttpContext.Current.Server.MapPath(DASHBOARD_CONFIG);
                xml.Save(configPath);
            }
        }
    }
}
