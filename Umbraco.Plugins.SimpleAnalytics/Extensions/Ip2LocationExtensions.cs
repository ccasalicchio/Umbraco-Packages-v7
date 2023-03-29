using IP2Location;

using Umbraco.Plugins.SimpleAnalytics.Models;

namespace Umbraco.Plugins.SimpleAnalytics.Extensions
{
    public static class Ip2LocationExtensions
    {
        public static IPMapping GetIpMapping(string ip, string binPath)
        {
            var location = new Component();
            location.Open(binPath);
            var result = location.IPQuery(ip);
            return new IPMapping
            {
                IPAddress = ip,
                City = result.City,
                Country = result.CountryLong,
                CountryCode = result.CountryShort,
                CountryFlag = result.CountryShort.ToLower() + ".png",
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                PostalCode = result.ZipCode,
                NetSpeed = result.NetSpeed,
                ISP = result.InternetServiceProvider,
                Region = result.Region,
                TimeZone = result.TimeZone
            };
        }
    }
}
