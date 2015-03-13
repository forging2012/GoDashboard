using System.Collections.Generic;
using GoDashboard.Web.ViewModels;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.ViewModels
{
    [TestFixture]
    public class DisplayablePipelineTests
    {
        [Test]
        public void santized_name_should_remove_dots_from_name()
        {
            var displayablePipeline = new DisplayablePipeline("A.Name.With.Dots", null);

            Assert.That(displayablePipeline.SanitizedName, Is.EqualTo("ANameWithDots"));
        }

        [Test]
        public void NameClass_should_be_long_if_hiding_build_info()
        {
            var displayablePipeline = new DisplayablePipeline("A.Name", null)
                {
                    HideBuildInfo = true
                };

            Assert.That(displayablePipeline.NameClass, Is.EqualTo("class=\"long\""));
        }
        
        [Test]
        public void NameClass_should_be_blank_if_not_hiding_build_info()
        {
            var displayablePipeline = new DisplayablePipeline("A.Name", null)
                {
                    HideBuildInfo = false
                };

            Assert.That(displayablePipeline.NameClass, Is.EqualTo(string.Empty));
        }      
        
        [Test]
        public void ProcessedDisplayName_should_be_trunctated_to_23_characters_if_name_is_longer_that_27_characters_and_hiding_build_info()
        {
            var displayablePipeline = new DisplayablePipeline("Abcdefghijklmnopqrstuvwxyz1", null)
                {
                    HideBuildInfo = true
                };

            Assert.That(displayablePipeline.ProcessedDisplayName, Is.EqualTo("Abcdefghijklmnopqrstuvwx…"));
        }
        
        [Test]
        public void ProcessedDisplayName_should_not_be_trunctated_if_name_is_less_that_16_characters_and_not_hiding_build_info()
        {
            var displayablePipeline = new DisplayablePipeline("Abcdefghijklmno", null)
                {
                    HideBuildInfo = false
                };

            Assert.That(displayablePipeline.ProcessedDisplayName, Is.EqualTo("Abcdefghijklmno"));
        }

        [Test]
        public void ProcessedDisplayName_should_be_trunctated_to_16_characters_if_name_is_more_than_15_characters_and_not_hiding_build_info()
        {
            var displayablePipeline = new DisplayablePipeline("Abcdefghijklmnop", null)
            {
                HideBuildInfo = false
            };

            Assert.That(displayablePipeline.ProcessedDisplayName, Is.EqualTo("Abcdefghijklmno…"));
        }
        
        [Test]
        public void ProcessedDisplayName5()
        {
            var displayablePipeline = new DisplayablePipeline("Abcdefghijklmnopqrstuvwxyz", null)
                {
                    HideBuildInfo = true
                };

            Assert.That(displayablePipeline.ProcessedDisplayName, Is.EqualTo("Abcdefghijklmnopqrstuvwxyz"));
        }

        [Test]
        public void SetsStyleWidth()
        {
            var displayablePipeline = new DisplayablePipeline("Name", null)
                {
                    Stages = new List<IStage>
                        {
                            new PassedStage("Stage 1", null, "2000-01-01"),
                            new PassedStage("Stage 2", null, "2000-01-01")
                        }
                };

            Assert.That(displayablePipeline.PipelineStageWidth, Is.EqualTo(48));
        }

        [Test]
        public void should_not_truncate_last_build_label_if_less_than_16_characters()
        {
            var displayablePipeline = new DisplayablePipeline("Name", "123456789012345")
                {
                    Stages = new List<IStage>
                        {
                            new PassedStage("Stage 1", null, "2000-01-01")
                        }
                };

            Assert.That(displayablePipeline.ProcessedLastBuildLabel, Is.EqualTo("123456789012345"));              
        }

        [Test]
        public void should_truncate_last_build_label_if_more_than_15_characters()
        {
            var displayablePipeline = new DisplayablePipeline("Name", "1234567890123456")
                {
                    Stages = new List<IStage>
                        {
                            new PassedStage("Stage 1", null, "2000-01-01")
                        }
                };

            Assert.That(displayablePipeline.ProcessedLastBuildLabel, Is.EqualTo("123456789012345…"));              
        }
    }
}
