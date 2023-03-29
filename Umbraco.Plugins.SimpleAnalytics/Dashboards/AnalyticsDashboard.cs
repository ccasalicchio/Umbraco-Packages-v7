
using Umbraco.Tools.ConfigurationActions.Modules;

namespace Umbraco.Plugins.SimpleAnalytics.Dashboards
{
    public class AnalyticsDashboard : IDashboard
    {
        public string Alias => "SimpleAnalytics";
        public string[] Areas => new string[] { "content", "settings" };
        public string TabCaption => "Analytics";
        public string Control => "/App_Plugins/VisitCounterDashboard/views/view.html";
        public string[] AccessDeny => new string[] { "translators"};
        public string[] AccessGrant => new string[] { "admin", "editor" };
    }
}
