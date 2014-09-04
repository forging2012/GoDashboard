using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoDashboard.Web.Controllers;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Controller
{
    [TestFixture]
    public class TestRefreshInHomeController
    {
        [Test]
        public void RefreshReturnsRefreshView()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Web", "bob")
                                        };

            var result =
                (ViewResult)
                new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader()).Refresh("");
            Assert.That(result.ViewName, Is.EqualTo("Refresh"));
        }

        [Test]
        public void RefreshReturnsViewModelWithPipelines()
        {
            var expectedPipelines = new List<DisplayablePipeline> { new DisplayablePipeline("Web", "bob") };

            var viewResult =
                (ViewResult)
                new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader()).Refresh("");

            var actualViewModel = (IndexViewModel)viewResult.ViewData.Model;
            Assert.That(actualViewModel.Pipelines.Count(), Is.EqualTo(expectedPipelines.Count()));
            Assert.That(actualViewModel.Pipelines.First().Name, Is.EqualTo(expectedPipelines.First().Name));
        }

        [Test]
        public void Refresh_Returns_Model_With_Passed_Pipeline_Count()
        {
            var expectedPipelines = new List<DisplayablePipeline>
                                        {
                                            new DisplayablePipeline("Shared", "bob"){Stages = new List<IStage>(){new PassedStage("", "", DateTime.Now.ToString())}},
                                            new DisplayablePipeline("Shared2", "bob2"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString())}},
                                            new DisplayablePipeline("Shared3", "bob3"){Stages = new List<IStage>(){new PassedStage("", "", DateTime.Now.ToString())}}
                                        };

            var homeController = new HomeController(new FakeDashboard(expectedPipelines), new FakeXmlProfileLoader());
            var result = (ViewResult)homeController.Refresh("");
            var viewModel = (IndexViewModel)result.ViewData.Model;

            Assert.That(viewModel.PassedCount, Is.EqualTo(2));
        }
    }
}