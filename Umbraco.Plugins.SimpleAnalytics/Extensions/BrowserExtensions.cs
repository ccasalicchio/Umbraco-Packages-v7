
using Ng.Services;

namespace Umbraco.Plugins.SimpleAnalytics.Extensions
{
    public static class BrowserExtensions
    {
        public static string ParseOS(string platform)
        {
            if (platform.ToLower().Contains("windows")) return "windows.png";
            if (platform.ToLower().Contains("mac")) return "mac.png";
            if (platform.ToLower().Contains("linux")) return "linux.png";
            if (platform.ToLower().Contains("ios")) return "ios.png";
            if (platform.ToLower().Contains("android")) return "android.png";
            if (platform.ToLower().Contains("chrome")) return "chromeos.png";
            return "";
        }

        public static UserAgent GetBrowserInfo(string userAgentString)
        {
            var service = new UserAgentService();

            return service.Parse(userAgentString);
        }
    }
}
