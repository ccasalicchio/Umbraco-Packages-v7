namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    public class BrowserInfo
    {
        public string AppVersion { get; set; }
        public string Language { get; set; }
        public string Platform { get; set; }
        public string Os { get; set; }
        public string UserAgent { get; set; }
        public BrandVersion[] Versions { get; set; }
        public string Vendor { get; set; }
    }
}
