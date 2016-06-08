using SocialMediaChannels.Helpers;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace SocialMediaChannels
{
    public class AddSocialMediaTheme : IPackageAction
    {
        #region IPackageAction Methods
        public string Alias()
        {
            return "SocialMediaChannels_AddSocialMediaTheme";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            string theme = XmlHelper.GetAttributeValueFromNode(xmlData, "theme");
            MediaHelper.AddSocialMediaChannelTheme(theme);
            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            string theme = XmlHelper.GetAttributeValueFromNode(xmlData, "theme");
            MediaHelper.RemoveSocialMediaChannelTheme(theme);
            return true;
        }

        public XmlNode SampleXml()
        {
            var xml = string.Format("<Action runat=\"install\" undo=\"true\" alias=\"{0}\" theme=\"theme folder\" />", Alias());
            return helper.parseStringToXmlNode(xml);
        }
        #endregion
    }
}