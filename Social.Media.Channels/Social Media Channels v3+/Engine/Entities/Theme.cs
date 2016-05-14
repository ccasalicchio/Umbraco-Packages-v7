using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Social_Media_Channels.Entities
{
    [DataContract]
    [XmlRoot(Namespace = "http://zueuz.com/Theme-Schema.xsd", ElementName = "theme", IsNullable = false)]
    public class Theme
    {
        [XmlElement("channel")]
        [DataMember(Name="Channels")]
        public List<Channel> Channels { get; set; }

        [XmlAttribute("id")]
        public string ID { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("created-date")]
        public string CreatedDate { get; set; }
        [XmlAttribute("created-by")]
        public string Creator { get; set; }
        [XmlAttribute("url-reference")]
        public string Url { get; set; }

        [XmlIgnore]
        public string Path { get; set; }

        public override string ToString()
        {
            return this.ID;
        }
        
    }
}