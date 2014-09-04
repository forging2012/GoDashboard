using System.Xml.Linq;

namespace GoDashboard.Web.Modules.Interfaces
{
    public interface IXmlProfileRetriever
    {
        XElement GetProfileXml(string profileName);
    }
}