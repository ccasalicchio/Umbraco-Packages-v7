using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Umbraco.Plugins.SocialMediaChannels.Models
{
    [XmlRoot("theme", Namespace = "http://zueuz.com/Theme-Schema.xsd")]
    public class SocialMediaChannelThemeModel
    {

        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("created-date")]
        public DateTime CreateDate { get; set; }
        [XmlAttribute("created-by")]
        public string CreatedBy { get; set; }
        [XmlAttribute("url-reference")]
        public string Reference { get; set; }

        [XmlElement("channel")]
        public List<SocialMediaChannel> Channels { get; set; }
        public bool ShowLabels { get; set; }
    }

    public class SocialMediaChannel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("image")]
        public string Image { get; set; }
        public string Url { get; set; }
    }

    public class SocialMediaChannelPackage
    {
        public string Name { get; set; }
        public SocialMediaChannelThemeModel Theme { get; set; }
        public string Thumbnail { get; set; }
        public string Folder { get; set; }
    }
}
