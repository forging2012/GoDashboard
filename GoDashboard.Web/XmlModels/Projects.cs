using System.Collections.Generic;
using System.Xml.Serialization;

namespace GoDashboard.Web.XmlModels
{
    [XmlRoot("Projects")]
    public class Projects
    {
        [XmlElement("Project")]
        public List<Project> Results { get; set; }
    }
}