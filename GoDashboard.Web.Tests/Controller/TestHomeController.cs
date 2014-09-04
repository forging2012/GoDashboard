using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using GoDashboard.Web.Controllers;
using GoDashboard.Web.Models;
using GoDashboard.Web.Modules;
using GoDashboard.Web.Modules.Interfaces;
using GoDashboard.Web.Tests.Fakes;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Controller
{
    [TestFixture]
    public class TestIndexInHomeController
    {
        [Test]
        public void Index_Returns_Model_With_Pipeline()
        {

            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", ""){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Shared</Pipeline></Group></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "<ShowPassedCount>false</ShowPassedCount>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.Pipelines.First().Name, Is.EqualTo("Shared"));
        }

        [Test]
        public void Index_Returns_Model_With_Passed_Pipeline_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob2"){Stages = new List<IStage>{new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob3"){Stages = new List<IStage>{new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader());
            var result = (ViewResult)homeController.Index("");
            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.PassedCount, Is.EqualTo(2));
        }

        [Test]
        public void Should_Count_Passed_Based_On_Profile_Name()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Pipeline1", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                                           + "<Profiles>"
                                           + "<Profile name=\"test\">"
                                           + "<WhiteList>"
                                           + "<Group>"
                                           + "<Pipeline>Pipeline1</Pipeline>"
                                           + "<Pipeline>Pipeline2</Pipeline>"
                                           + "<Pipeline>Pipeline3</Pipeline>"
                                           + "<Pipeline>Pipeline4</Pipeline>"
                                           + "</Group>"
                                           + "</WhiteList>"
                                           + "<Statuses>"
                                           + "<Failed/><Passed/>"
                                           + "</Statuses>"
                                           + "</Profile>"
                                           + "<Profile name=\"test2\">"
                                           + "<WhiteList>"
                                           + "<Group>"
                                           + "<Pipeline>Pipeline5</Pipeline>"
                                           + "<Pipeline>Pipeline6</Pipeline>"
                                           + "<Pipeline>Pipeline7</Pipeline>"
                                           + "<Pipeline>Pipeline8</Pipeline>"
                                           + "</Group>"
                                           + "</WhiteList>"
                                           + "<Statuses>"
                                           + "<Failed/><Passed/>"
                                           + "</Statuses>"
                                           + "</Profile>"
                                           + "</Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.PassedCount, Is.EqualTo(2));
        }

        [Test]
        public void Should_Show_Passed_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></Group></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "<ShowPassedCount>true</ShowPassedCount>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(true));
        }

        [Test]
        public void Should_Not_Show_Passed_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></Group></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "<ShowPassedCount>false</ShowPassedCount>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(false));
        }

        [Test]
        public void Should_Not_Show_Passed_Count_If_Not_Specified()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></Group></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(false));
        }

        [Test]
        public void Index_Returns_Model_With_Pipeline_Groups()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Pipeline1", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList>"
                                           + "<Group name=\"Group1\">"
                                           + "<Pipeline>Pipeline1</Pipeline>"
                                           + "<Pipeline>Pipeline2</Pipeline>"
                                           + "</Group>"
                                           + "<Group name=\"Group2\">"
                                           + "<Pipeline>Pipeline3</Pipeline>"
                                           + "<Pipeline>Pipeline4</Pipeline>"
                                           + "</Group>"
                                           + "</WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.Groups.Count(), Is.EqualTo(2));
            Assert.That(viewModel.Groups[0].Pipelines.Count, Is.EqualTo(2));
            Assert.That(viewModel.Groups[1].Pipelines.Count, Is.EqualTo(2));

        } 
        
        [Test]
        public void Index_Returns_Model_With_Pipeline_Groups_With_ShowName_Flag()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Pipeline1", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList>"
                                           + "<Group name=\"Group1\" showName=\"true\">"
                                           + "<Pipeline>Pipeline1</Pipeline>"
                                           + "<Pipeline>Pipeline2</Pipeline>"
                                           + "</Group>"
                                           + "<Group name=\"Group2\" showName=\"false\">"
                                           + "<Pipeline>Pipeline3</Pipeline>"
                                           + "<Pipeline>Pipeline4</Pipeline>"
                                           + "</Group>" 
                                           + "<Group name=\"Group3\">"
                                           + "<Pipeline>Pipeline5</Pipeline>"
                                           + "<Pipeline>Pipeline6</Pipeline>"
                                           + "</Group>"
                                           + "</WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.Groups.Count(), Is.EqualTo(3));
            Assert.That(viewModel.Groups[0].ShowName, Is.True);
            Assert.That(viewModel.Groups[1].ShowName, Is.False);
            Assert.That(viewModel.Groups[2].ShowName, Is.True);

        }
    }

    [TestFixture]
    public class TestRefreshProfiling
    {
        [Test]
        public void IndexReturnsModelWithPipeline()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob")
                                        };

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader());
            var result = (ViewResult)homeController.Refresh("");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            var pipeline = viewModel.Pipelines.First();

            Assert.That(pipeline.Name, Is.EqualTo("Shared"));
        }

        [Test]
        public void Index_Returns_Model_With_Passed_Pipeline_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob2"){Stages = new List<IStage>{new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob3"){Stages = new List<IStage>{new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader());
            var result = (ViewResult)homeController.Refresh("");
            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.PassedCount, Is.EqualTo(2));
        }

        [Test]
        public void Should_Count_Passed_Based_On_Profile_Name()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Pipeline1", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Pipeline4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles>"
                                           + "<Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></Group></WhiteList>"
                                           + "<Statuses>"
                                           + "<Failed/><Passed/>"
                                           + "</Statuses>"
                                           + "</Profile>"
                                           + "<Profile name=\"test2\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline5</Pipeline><Pipeline>Pipeline6</Pipeline><Pipeline>Pipeline7</Pipeline><Pipeline>Pipeline8</Pipeline></Group></WhiteList>"
                                           + "<Statuses>"
                                           + "<Failed/><Passed/>"
                                           + "</Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Refresh("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.PassedCount, Is.EqualTo(2));
        }

        [Test]
        public void Should_Show_Passed_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "<ShowPassedCount>true</ShowPassedCount>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Refresh("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(true));
        }

        [Test]
        public void Should_Not_Show_Passed_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "<ShowPassedCount>false</ShowPassedCount>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Refresh("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(false));
        }

        [Test]
        public void Should_Not_Show_Passed_Count_If_Not_Specified()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared4", "bob"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles><Profile name=\"test\">"
                                           + "<WhiteList><Group><Pipeline>Pipeline1</Pipeline><Pipeline>Pipeline2</Pipeline><Pipeline>Pipeline3</Pipeline><Pipeline>Pipeline4</Pipeline></Group></WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Refresh("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.ShowPassedCount, Is.EqualTo(false));
        }
    }
    public class FakeXmlProfileLoader : IXmlProfileRetriever
    {
        public XElement GetProfileXml(string profileName)
        {
            throw new NotImplementedException();
        }
    }


    public class FakeDashboard : IDashboard
    {
        private readonly List<DisplayablePipeline> expectedPipelines;

        public FakeDashboard(List<DisplayablePipeline> expectedPipelines)
        {
            this.expectedPipelines = expectedPipelines;
        }

        public FakeDashboard()
        {
            this.expectedPipelines = new List<DisplayablePipeline>();
        }

        public PipelineGroup GetPipelinesFromServer()
        {
            return new PipelineGroup();
        }

        public XmlDocument GetPipelinesXmlFromServer()
        {
            return new XmlDocument();
        }

        public List<PipelineGroup> CompressPipelineGroup(PipelineGroup pipelineGroup)
        {
            return new List<PipelineGroup>();
        }

        public List<DisplayablePipeline> Pipelines()
        {
            return this.expectedPipelines;
        }

        public List<DisplayablePipeline> PipelinesTwo()
        {
            return null;
        }
    }
}