using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GoDashboard.Web.Controllers;
using GoDashboard.Web.Modules;
using GoDashboard.Web.Modules.Interfaces;
using GoDashboard.Web.Tests.Fakes;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Controller
{
    [TestFixture]
    public class TestHomeControllerProfiling
    {
        [Test]
        public void Should_Filter_Index_Based_On_Profile_Name()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage> {new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage> {new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage> {new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                        };

            const string ProfilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Profiles>"
                                           + "<Profile name=\"test\">"
                                           + "<WhiteList>" 
                                           + "<Pipeline>Shared</Pipeline>"
                                           + "<Pipeline>Shared2</Pipeline>"
                                           + "<Pipeline>Shared3</Pipeline>"
                                           + "</WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile>"
                                           + "<Profile name=\"test2\">"
                                           + "<WhiteList>" 
                                           + "<Pipeline>Shared4</Pipeline>"
                                           + "<Pipeline>Shared5</Pipeline>"
                                           + "<Pipeline>Shared6</Pipeline>"
                                           + "</WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(ProfilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Index("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            var pipelines = viewModel.Pipelines;
            var pipeline = pipelines.First();

            Assert.That(pipelines.Count(), Is.EqualTo(3));
            Assert.That(pipeline.Name, Is.EqualTo("Shared"));

        }

        [Test]
        public void Should_Filter_Refresh_Based_On_Profile_Name()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>(){new FailedStage("","",DateTime.Now.ToString())}},
                                            new DisplayablePipeline("Shared2", "bob"){Stages = new List<IStage>(){new PassedStage("","",DateTime.Now.ToString())}},
                                            new DisplayablePipeline("Shared3", "bob"){Stages = new List<IStage>(){new FailedStage("","",DateTime.Now.ToString())}}
                                        };

            const string profilesXmlFile = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
                                           + "<Profiles>"
                                           + "<Profile name=\"test\">"
                                          + "<WhiteList>"
                                           + "<Pipeline>Shared</Pipeline>"
                                           + "<Pipeline>Shared2</Pipeline>"
                                           + "<Pipeline>Shared3</Pipeline>"
                                           + "</WhiteList>"
                                           + "<Statuses><Failed/><Passed/></Statuses>"
                                           + "</Profile></Profiles>";

            var fakeFileLoader = new FakeFileLoader(profilesXmlFile);

            IXmlProfileRetriever xmlProfileRetriever = new XmlProfileRetriever(fakeFileLoader);

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), xmlProfileRetriever);
            var result = (ViewResult)homeController.Refresh("test");

            var viewModel = (IndexViewModel)result.ViewData.Model;

            var pipelines = viewModel.Pipelines;     
            Assert.That(pipelines.Count(), Is.EqualTo(3));
            
            var pipeline = pipelines.First();
            Assert.That(pipeline.Name, Is.EqualTo("Shared"));
        }
    }
}