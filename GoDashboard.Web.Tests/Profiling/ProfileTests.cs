using System;
using System.Linq;
using GoDashboard.Web.Models;
using GoDashboard.Web.Modules;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;
using System.Collections.Generic;

namespace GoDashboard.Web.Tests.Profiling
{
    [TestFixture]
    public class ProfileTests
    {
        [Test]
        public void Should_Filter_By_Pipeline_Status_Failed()
        {
            var profile = new Profile
            {
                Statuses = new List<PipelineStatus> { PipelineStatus.Failed },

                PipelineGroups = {new GroupedPipeline{
                    Name = "Group",
                    ProfilePipelines = new List<ProfilePipeline>
                        {
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline1"},
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline2"}
                        },
                    }}

            };

            var pipelines = new List<DisplayablePipeline>
                                {
                                    new DisplayablePipeline("Pipeline1", "0000"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline2", "0000"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                };


            var pipelineFilter = new PipelineFilter(pipelines);

            var filteredPipelines = pipelineFilter.Filter(profile);

            Assert.That(filteredPipelines.Count(), Is.EqualTo(1));
            Assert.That(filteredPipelines[0].Pipelines.Count, Is.EqualTo(1));
            Assert.That(filteredPipelines[0].Pipelines[0].Name, Is.EqualTo("Pipeline2"));
        }

        [Test]
        public void Should_Filter_By_Pipeline_Status_Passed()
        {
            var profile = new Profile
            {
                Statuses = new List<PipelineStatus> { PipelineStatus.Passed },
                PipelineGroups = { new GroupedPipeline { Name = "Group", ProfilePipelines = new List<ProfilePipeline>
                        {
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline1"},
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline2"}
                        },
                       } }

            };

            var pipelines = new List<DisplayablePipeline>
                                {
                                    new DisplayablePipeline("Pipeline1", "0000"){Stages = new List<IStage>{new PassedStage("","",DateTime.Now.ToString())}},
                                    new DisplayablePipeline("Pipeline2", "0000"){Stages = new List<IStage>{new FailedStage("","",DateTime.Now.ToString())}}
                                };


            var pipelineFilter = new PipelineFilter(pipelines);

            var filteredPipelines = pipelineFilter.Filter(profile);

            Assert.That(filteredPipelines[0].Pipelines.Count, Is.EqualTo(1));
            Assert.That(filteredPipelines[0].Pipelines[0].Name, Is.EqualTo("Pipeline1"));

        }

        [Test]
        public void Should_WhiteList_By_Pipeline_Names()
        {
            var group = new GroupedPipeline
            {
                Name = "Name",
                ProfilePipelines = new List<ProfilePipeline>
                        {
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline1"},
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline2"}
                        }
                
            };
            var profile = new Profile
            {
                PipelineGroups = new List<GroupedPipeline> { group }
            };

            var pipelines = new List<DisplayablePipeline>
                                {
                                    new DisplayablePipeline("Pipeline1", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline2", "failed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline3", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline4", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                };


            var pipelineFilter = new PipelineFilter(pipelines);

            var filteredPipelines = pipelineFilter.Filter(profile);

            Assert.That(filteredPipelines[0].Pipelines.Count, Is.EqualTo(2));
            Assert.That(filteredPipelines[0].Pipelines[0].Name, Is.EqualTo("Pipeline1"));
            Assert.That(filteredPipelines[0].Pipelines[1].Name, Is.EqualTo("Pipeline2"));
        }

        [Test]
        public void Should_Filter_WhiteListGroup_By_Pipeline_Names()
        {
            var group = new GroupedPipeline
            {
                Name = "Name",
                ProfilePipelines = new List<ProfilePipeline>
                        {
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline1"},
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline2"}
                        }
            };
            var group2 = new GroupedPipeline
            {
                Name = "Name2",
                ProfilePipelines = new List<ProfilePipeline>
                        {
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline3"},
                            new ProfilePipeline{HideBuildInfo = true, Name = "Pipeline4"}
                        }
            };
            var profile = new Profile
            {
                PipelineGroups = new List<GroupedPipeline> { group, group2 }
            };

            var pipelines = new List<DisplayablePipeline>
                                {
                                    new DisplayablePipeline("Pipeline1", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline2", "failed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline3", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline4", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline5", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline6", "passed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                };


            var pipelineFilter = new PipelineFilter(pipelines);

            var filteredPipelines = pipelineFilter.Filter(profile);


            Assert.That(filteredPipelines.Count(), Is.EqualTo(2));
            Assert.That(filteredPipelines[0].Pipelines.Count, Is.EqualTo(2));
            Assert.That(filteredPipelines[1].Pipelines.Count, Is.EqualTo(2));
            Assert.That(filteredPipelines[0].Pipelines[0].Name, Is.EqualTo("Pipeline1"));
            Assert.That(filteredPipelines[0].Pipelines[1].Name, Is.EqualTo("Pipeline2"));
            Assert.That(filteredPipelines[1].Pipelines[0].Name, Is.EqualTo("Pipeline3"));
            Assert.That(filteredPipelines[1].Pipelines[1].Name, Is.EqualTo("Pipeline4"));
        }

        [Test]
        public void Should_Filter_All_Pipelines_By_Status_If_No_Whitelist()
        {
            var profile = new Profile
            {
                Statuses = new List<PipelineStatus> { PipelineStatus.Failed }
            };

            var pipelines = new List<DisplayablePipeline>
                                {
                                    new DisplayablePipeline("Pipeline1", "passed"){Stages = new List<IStage>(){new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline2", "failed"){Stages = new List<IStage>(){new FailedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}},
                                    new DisplayablePipeline("Pipeline3", "passed"){Stages = new List<IStage>(){new PassedStage("", "", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"))}}
                                };


            var pipelineFilter = new PipelineFilter(pipelines);

            var filteredPipelines = pipelineFilter.Filter(profile);


            Assert.That(filteredPipelines.Count(), Is.EqualTo(1));
            Assert.That(filteredPipelines[0].Pipelines[0].Name, Is.EqualTo("Pipeline2"));
        }
    }
}
