using SocialMediaChannels.Helpers;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace SocialMediaChannels
{
    public class AddSocialMediaThemes : IPackageAction
    {
        #region IPackageAction Methods
        public string Alias()
        {
            return "SocialMediaChannels_AddSocialMediaThemes";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            MediaHelper.AddSocialMediaChannelMediaType();
            MediaHelper.AddSocialMediaChannelThemes();
            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            MediaHelper.RemoveSocialMediaChannelMediaType();
            MediaHelper.RemoveSocialMediaChannelThemes();
            return true;
        }

        public XmlNode SampleXml()
        {
            var xml = string.Format("<Action runat=\"install\" undo=\"true\" alias=\"{0}\" />", Alias());
            return helper.parseStringToXmlNode(xml);
        }
        #endregion
    }
}