namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    public class VisitStats
    {
        public int NodeId{ get; set; }
        public string EntryUrl { get; set; }
        public string IPAddress { get; set; }
        public string Filter { get; set; }
        public int Count { get; set; }
        public string ExitUrl { get; set; }
        public IPMapping Mapping { get; set; }
    }
}
