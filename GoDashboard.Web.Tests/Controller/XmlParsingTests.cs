using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using GoDashboard.Web.Construction;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Controller
{
    [TestFixture]
    public class XmlParsingTests
    {
        private string xml =
            @"<Projects>
                <Project name='project-one :: build' lastBuildStatus='Success' activity='Building' lastBuildLabel='77777' lastBuildTime='2012-01-24T10:05:34' />
                <Project name='project-two :: build' lastBuildStatus='Success' activity='Sleeping' lastBuildLabel='77888' lastBuildTime='2012-01-25T10:05:34' />
                <Project name='project-two :: deploy' lastBuildStatus='Failure' activity='Sleeping' lastBuildLabel='77888' lastBuildTime='2012-01-26T10:05:34' />
              </Projects>";

        [Test]
        public void GetProjects_GetsTwoProjects_FromXml()
        {
            var result = GetrojectsFrom(xml);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetProjects_SetsStatus_OfPipeline()
        {
            var result = GetrojectsFrom(xml);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Status, Is.EqualTo("building"));
            Assert.That(result.Last().Status, Is.EqualTo("failed"));
        }

        [Test]
        public void GetProjects_SetsLastBuildLabel_OfPipeline()
        {
            var result = GetrojectsFrom(xml);
            Assert.That(result.First().LastBuildLabel, Is.EqualTo("77777"));
        }

        [Test]
        public void GetProjects_SetsStatus_OfStages()
        {
            var result = GetrojectsFrom(xml);
            var stages = result.Last().Stages;
            Assert.That(stages.First().Status, Is.EqualTo("passed"));
            Assert.That(stages.Last().Status, Is.EqualTo("failed"));
        }

        [Test]
        public void GetProjects_SetsActivity_OfStages()
        {
            var result = GetrojectsFrom(xml);
            var stages = result.First().Stages;
            Assert.That(stages.First().Activity, Is.EqualTo("Building"));
        }

        [Test]
        public void GetProjects_SetsFixOverdue_WhenPipelineIsBroken()
        {
            var result = GetrojectsFrom(xml);
            Assert.That(result.First().FixOverdue, Is.EqualTo("good"));
            Assert.That(result.Last().FixOverdue, Is.EqualTo("overdue"));
        }

        [Test]
        public void GetProjects_SetsLastBuildTime_ToLatestTime()
        {
            var result = GetrojectsFrom(xml);
            Assert.That(result.First().LastBuildTime, Is.EqualTo(new DateTime(2012, 1, 24, 10, 5, 34)));
            Assert.That(result.Last().LastBuildTime, Is.EqualTo(new DateTime(2012, 1, 26, 10, 5, 34)));
        }

        private List<DisplayablePipeline> GetrojectsFrom(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return PipelineDeserialiser.DeserialisePipelinesFromXmlAndCleanseContent(xmlDoc);
        }
    }
}
