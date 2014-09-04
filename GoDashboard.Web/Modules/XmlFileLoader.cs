using System.Xml.Linq;
using GoDashboard.Web.Modules.Interfaces;

namespace GoDashboard.Web.Modules
{
    public class XmlFileLoader : IFileLoader
    {
        private readonly string _xmlFile;

        public XmlFileLoader(string xmlFile)
        {
            _xmlFile = xmlFile;
        }

        public string Load()
        {
            var xmlFileLocation = System.Web.HttpContext.Current.Server.MapPath(_xmlFile);
            return XElement.Load(xmlFileLocation).ToString();
        }
    }
}