using System.Xml.Linq;
using GoDashboard.Web.Modules;
using GoDashboard.Web.Tests.Fakes;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Profiling
{
    [TestFixture]
    public class RetrieveProfileXmlTests
    {
        [Test]
        public void Should_Load_Xml_For_Name()
        {
            const string profilesXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                       + "<WhiteList>"
                                       + "<Pipeline>Pipeline1</Pipeline>"
                                       + "<Pipeline>Pipeline2</Pipeline>"
                                       + "</WhiteList>"
                                       + "<Statuses>"
                                       + "<Failed/></Statuses></Profile></Profiles>";

            IFileLoader fakeFileLoader = new FakeFileLoader(profilesXml);

            var xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var profileXml = xmlProfileRetriever.GetProfileXml("test");

            Assert.That(profileXml.ToString(), Is.EqualTo(XElement.Parse("<Profile name=\"test\">"
                                                                        + "<WhiteList>"
                                                                        + "<Pipeline>Pipeline1</Pipeline>"
                                                                        + "<Pipeline>Pipeline2</Pipeline>"
                                                                        + "</WhiteList>"
                                                                        + "<Statuses><Failed/></Statuses>"
                                                                        + "</Profile>").ToString()));
        }

        [Test]
        public void Should_Load_Xml_For_Name_without_caring_about_case()
        {
            const string profilesXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                                       + "<Profiles><Profile name=\"test\">"
                                       + "<WhiteList>"
                                       + "<Pipeline>Pipeline1</Pipeline>"
                                       + "<Pipeline>Pipeline2</Pipeline>"
                                       + "</WhiteList>"
                                       + "<Statuses>"
                                       + "<Failed/></Statuses></Profile></Profiles>";

            IFileLoader fakeFileLoader = new FakeFileLoader(profilesXml);

            var xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var profileXml = xmlProfileRetriever.GetProfileXml("Test");

            Assert.That(profileXml.ToString(), Is.EqualTo(XElement.Parse("<Profile name=\"test\">"
                                                                         + "<WhiteList>"
                                                                         + "<Pipeline>Pipeline1</Pipeline>"
                                                                         + "<Pipeline>Pipeline2</Pipeline>"
                                                                         + "</WhiteList>"
                                                                         + "<Statuses><Failed/></Statuses>"
                                                                         + "</Profile>").ToString()));
        }

        [Test]
        public void Should_Return_Null_for_NonExistant_Profile()
        {
            const string profilesXml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                       + "<WhiteList>"
                                       + "<Pipeline>Pipeline1</Pipeline>"
                                       + "<Pipeline>Pipeline2</Pipeline>"
                                       + "</WhiteList>"
                                       + "<Statuses><Failed/></Statuses>"
                                       + "</Profile></Profiles>";

            IFileLoader fakeFileLoader = new FakeFileLoader(profilesXml);

            var xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var profileXml = xmlProfileRetriever.GetProfileXml("aaa");

            Assert.That(profileXml, Is.Null);
        }
    }
}