using System.Xml.Serialization;

namespace GoDashboard.Web.XmlModels
{
    public class Project
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("lastBuildStatus")]
        public string LastBuildStatus { get; set; }
        [XmlAttribute("activity")]
        public string Activity { get; set; }
        [XmlAttribute("lastBuildLabel")]
        public string LastBuildLabel { get; set; }
        [XmlAttribute("lastBuildTime")]
        public string LastBuildTime { get; set; }
    }
}