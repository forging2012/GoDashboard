using System.Linq;
using GoDashboard.Web.Modules;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Module
{
    [TestFixture]
    public class TestDashboard
    {
        [Test]
        public void TestPipelinesReturnsSomething()
        {
            const string content = "<?xml version='1.0' encoding='utf-8'?><Projects>"
                                   + "<Project name='Shared-GoogleSiteSearch :: build'"
                                   + " activity='Sleeping' lastBuildStatus='Success'"
                                   + " lastBuildLabel='69807' lastBuildTime='2012-01-23T13:40:48'"
                                   + " webUrl='http://go.server.com:8153/go/pipelines/CI/1/build/1' />"
                                   + "</Projects>";

            DisplayablePipeline pipeline = new DashBoard(content).Pipelines().First();

            Assert.That(pipeline.Name, Is.EqualTo("Shared"));
        }
    }
}