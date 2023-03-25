using System.Collections.Generic;

using Umbraco.Plugins.SocialMediaChannels.Extensions;
using Umbraco.Plugins.SocialMediaChannels.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Umbraco.Plugins.SocialMediaChannels.Controllers
{
    [PluginController("SocialMedia")]
    public class SocialMediaChannelsController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<SocialMediaChannelPackage> GetThemes()
        {
            return UmbracoContext.HttpContext.GetPackages();
        }
    }
}
