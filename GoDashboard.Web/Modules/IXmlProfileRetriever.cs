using System.Xml.Linq;

namespace GoDashboard.Web.Controllers
{
    public interface IXmlProfileRetriever
    {
        XElement GetProfileXml(string profileName);
    }
}