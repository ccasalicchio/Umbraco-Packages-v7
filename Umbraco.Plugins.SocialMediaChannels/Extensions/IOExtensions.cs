using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;

using Umbraco.Plugins.SocialMediaChannels.Models;

namespace Umbraco.Plugins.SocialMediaChannels.Extensions
{
    public static class IOExtensions
    {
        private const string PATH = "/App_Plugins/SocialMediaChannels/themes";
        private const string VIRTUAL_PATH_SYMBOL = "~";
        public static IEnumerable<SocialMediaChannelPackage> GetPackages(this HttpContextBase context)
        {
            var path = context.Server.MapPath(VIRTUAL_PATH_SYMBOL + PATH);
            var thumbnails = Directory.GetFiles(path);
            List<SocialMediaChannelPackage> packages = new List<SocialMediaChannelPackage>();
            foreach (var thumbnail in thumbnails)
            {
                FileInfo fileInfo = new FileInfo(thumbnail);

                if (fileInfo.Extension != ".jpg") continue;

                var themeFolder = fileInfo.Name.Replace(".jpg", "");
                var theme = Deserialize(path, themeFolder);
                var package = new SocialMediaChannelPackage
                {
                    Thumbnail = $"{PATH}/{fileInfo.Name}",
                    Theme = theme,
                    Name = theme.Id,
                    Folder = themeFolder
                };

                packages.Add(package);
            }

            return packages;
        }


        private static SocialMediaChannelThemeModel Deserialize(string path, string themeFolder)
        {
            var themeConfig = Path.Combine(path, themeFolder, $"{themeFolder}.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(SocialMediaChannelThemeModel));
            SocialMediaChannelThemeModel result = null;
            using (FileStream fileStream = new FileStream(themeConfig, FileMode.Open))
            {
                result = (SocialMediaChannelThemeModel)serializer.Deserialize(fileStream);
            }
            return result;
        }
    }
}
