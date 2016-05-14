using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Social_Media_Channels.Entities
{
    [DataContract]
    [XmlRoot("channel")]
    public class Channel
    {
        [DataMember(Name="ID")]
        [XmlAttribute("id")]
        public string ID { get; set; }
        [DataMember(Name="Name")]
        [XmlAttribute("name")]
        public string Name { get; set; }
        [DataMember(Name="Image")]
        [XmlAttribute("image")]
        public string Image { get; set; }

        [XmlIgnore]
        [DataMember(Name="Url")]
        public string Url { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}