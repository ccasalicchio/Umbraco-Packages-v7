namespace Umbraco.Plugins.SimpleAnalytics.Models
{
    public class IPMapping
    {
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CountryFlag { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string PostalCode { get; set; }
        public string TimeZone { get; set; }
        public string Continent { get; set; }
        public string ISP { get; set; }
        public string NetSpeed { get; set; }
    }
}
