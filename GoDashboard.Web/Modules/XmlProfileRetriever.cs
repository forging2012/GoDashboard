using System.Linq;
using System.Xml.Linq;
using GoDashboard.Web.Controllers;

namespace GoDashboard.Web.Modules
{
    public class XmlProfileRetriever : IXmlProfileRetriever
    {
        private readonly IFileLoader _fileLoader;

        public XmlProfileRetriever(IFileLoader fileLoader)
        {
            _fileLoader = fileLoader;
        }

        public XElement GetProfileXml(string profileName)
        {
            var profilesXmlFileContent = _fileLoader.Load();
            var profilesXml = XElement.Parse(profilesXmlFileContent);
            var profiles = profilesXml.Elements("Profile").Where(x =>
            {
                var xAttribute = x.Attribute("name");
                return xAttribute != null && xAttribute.Value.ToLower() == profileName.ToLower();
            });
            return profiles.FirstOrDefault();
        }
    }
}