using System;

using GoDashboard.Web.Models;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Model
{
    [TestFixture]
    public class TestPipelineModel
    {
        [Test]
        public void Should_Set_LastBuildLabelInt_When_LastBuildLabel_Set()
        {
            var pipeline = new Pipeline(){LastBuildLabel = "10001"};
            Assert.AreEqual(pipeline.LastBuildLabelInt, 10001);
        }

        [Test]
        public void LastBuildLabelInt_Should_Not_Throw_Exception_When_LastBuildLabel_Not_Int()
        {
            var pipeline = new Pipeline() { LastBuildLabel = "v10001" };
            Assert.AreEqual(pipeline.LastBuildLabelInt, 0);
        }

        [Test]
        public void LastBuildLabelInt_Should_Not_Throw_Exception_When_LastBuildLabel_Is_Invalid()
        {
            var pipeline = new Pipeline() { LastBuildLabel = "${PipelineName}" };
            Assert.AreEqual(pipeline.LastBuildLabelInt, 0);
        }

        [Test]
        public void Status_Should_Be_Passed_If_LastBuildStatus_Passed()
        {
            var pipeline = new Pipeline() {LastBuildStatus = "Passed"};
            Assert.AreEqual(pipeline.Status, "passed");
        }

        [Test]
        public void Status_Should_Be_Failed_If_LastBuildStatus_Failed()
        {
            var pipeline = new Pipeline() { LastBuildStatus = "Failure" };
            Assert.AreEqual(pipeline.Status, "failed");
        }

        [Test]
        public void PipelineGroupName_Should_Be_What_It_Says_On_The_Tin()
        {
            var pipeline = new Pipeline() { Name = "Trunk-Blah-Blah_blag498234y9hwd-k[kgermeopupgapsd;a#'asc.';d.vs[" };
            Assert.AreEqual(pipeline.PipelineGroupName, "Trunk");
        }

        [Test]
        public void Should_Set_LastBuildTimeDateTime_If_Correct_DateTime()
        {
            var pipeline = new Pipeline() {LastBuildTimeDateTime = DateTime.Now};
            Assert.AreEqual((pipeline.LastBuildTimeDateTime - DateTime.Now).Minutes, 0);
        }
    }
}
