namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    public class AnalyticsVisitModel : AnalyticsVisit
    {
        public BrowserInfo Browser { get; set; }
        public IPMapping IPMapping { get; set; }
        public string NodeName { get; set; }
        public string EntryUrl { get; set; }
        public int TotalVisits { get; set; }
    }
}
