using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GoDashboard.Web.Models;
using GoDashboard.Web.Modules;
using NUnit.Framework;

namespace GoDashboard.Web.Tests.Profiling
{
    [TestFixture]
    public class ProfileLoaderTests
    {
        [Test]
        public void Should_Load_Profile_From_Xml()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                            + "<WhiteList>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline1</Pipeline>"
                                                 + "<Pipeline>Pipeline2</Pipeline>"
                                                + "</Group>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline3</Pipeline>"
                                                 + "<Pipeline>Pipeline4</Pipeline>"
                                                + "</Group>"
                                                + "</WhiteList>"
                                            + "<Statuses>"
                                            + "<Failed/>"
                                            + "</Statuses>"
                                            + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups.Count(), Is.EqualTo(2));
            Assert.That(profile.Statuses.Count(), Is.EqualTo(1));
            Assert.That(profile.Statuses.First(), Is.EqualTo(PipelineStatus.Failed));
        }

        [Test]
        public void Should_Load_Default_Profile_If_pass_in_null_XElement()
        {
            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(null);

            Assert.That(profile.PipelineGroups, Is.Null.Or.Empty);
            Assert.That(profile.Statuses.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_Load_ShowPassedCount_As_False_If_Not_Set_In_Xml()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                            + "<WhiteList>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline1</Pipeline>"
                                                 + "<Pipeline>Pipeline2</Pipeline>"
                                                + "</Group>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline3</Pipeline>"
                                                 + "<Pipeline>Pipeline4</Pipeline>"
                                                + "</Group>"
                                                + "</WhiteList>"
                                            + "<Statuses><Failed/></Statuses>"
                                            + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.ShowPassedCount, Is.EqualTo(false));
        }

        [Test]
        public void Should_Load_ShowPassedCount_As_True()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                            + "<WhiteList>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline1</Pipeline>"
                                                 + "<Pipeline>Pipeline2</Pipeline>"
                                                + "</Group>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline3</Pipeline>"
                                                 + "<Pipeline>Pipeline4</Pipeline>"
                                                + "</Group>"
                                                + "</WhiteList>"
                                            + "<Statuses><Failed/></Statuses>"
                                            + "<ShowPassedCount>true</ShowPassedCount>"
                                            + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.ShowPassedCount, Is.EqualTo(true));
        }

        [Test]
        public void Should_Load_Subgroups()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                            + "<WhiteList>"
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline1</Pipeline>"
                                                 + "<Pipeline>Pipeline2</Pipeline>"
                                                + "</Group>" 
                                                + "<Group>"
                                                 + "<Pipeline>Pipeline3</Pipeline>"
                                                 + "<Pipeline>Pipeline4</Pipeline>"
                                                + "</Group>"
                                                + "</WhiteList>"
                                            + "<Statuses><Failed/></Statuses>"
                                            + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups.Count, Is.EqualTo(2));
            Assert.That(profile.PipelineGroups[0].ProfilePipelines[0].Name, Is.EqualTo("Pipeline1"));
            Assert.That(profile.PipelineGroups[0].ProfilePipelines[1].Name, Is.EqualTo("Pipeline2"));
            Assert.That(profile.PipelineGroups[1].ProfilePipelines[0].Name, Is.EqualTo("Pipeline3"));
            Assert.That(profile.PipelineGroups[1].ProfilePipelines[1].Name, Is.EqualTo("Pipeline4"));
        }
        
        [Test]
        public void Subgroups_should_have_names()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
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
                                                        + "<Statuses><Failed/></Statuses>"
                                                        + "</Profile>");


            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups.Count, Is.EqualTo(2));
            Assert.That(profile.PipelineGroups[0].Name, Is.EqualTo("Group1"));
            Assert.That(profile.PipelineGroups[1].Name, Is.EqualTo("Group2"));
        }

        [Test]
        public void Subgroups_can_define_if_name_should_be_shown()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
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
                                                        + "<Statuses><Failed/></Statuses>"
                                                        + "</Profile>");


            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups.Count, Is.EqualTo(3));
            Assert.That(profile.PipelineGroups[0].ShowName, Is.True);
            Assert.That(profile.PipelineGroups[1].ShowName, Is.False);
            Assert.That(profile.PipelineGroups[2].ShowName, Is.True);
        }
        
        [Test]
        public void Pipelines_can_define_if_build_info_should_show()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                                        + "<WhiteList>"
                                                            + "<Group name=\"Group1\" showName=\"true\">"
                                                             + "<Pipeline>Pipeline1</Pipeline>"
                                                             + "<Pipeline hideBuildInfo=\"true\">Pipeline2</Pipeline>"
                                                            + "</Group>"
                                                            + "</WhiteList>"
                                                        + "<Statuses><Failed/></Statuses>"
                                                        + "</Profile>");


            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups.Count, Is.EqualTo(1));
            Assert.That(profile.PipelineGroups[0].ProfilePipelines[0].HideBuildInfo, Is.False);
            Assert.That(profile.PipelineGroups[0].ProfilePipelines[1].HideBuildInfo, Is.True);
        }

        [Test]
        public void Pipelines_can_define_an_alias()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                                        + "<WhiteList>"
                                                            + "<Group name=\"Group1\" showName=\"true\">"
                                                             + "<Pipeline>Pipeline1</Pipeline>"
                                                             + "<Pipeline alias=\"This is an alias\">Pipeline2</Pipeline>"
                                                            + "</Group>"
                                                            + "</WhiteList>"
                                                        + "<Statuses><Failed/></Statuses>"
                                                        + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml);

            Assert.That(profile.PipelineGroups[0].ProfilePipelines[0].Alias, Is.Null);
            Assert.That(profile.PipelineGroups[0].ProfilePipelines[1].Alias, Is.EqualTo("This is an alias"));
        }

        [Test]
        public void Only_show_requested_status_pipelines_even_if_statuses_say_otherwise()
        {
            var profileXml = XElement.Parse("<Profile name=\"test\">"
                                + "<WhiteList>"
                                    + "<Group>"
                                     + "<Pipeline>Pipeline1</Pipeline>"
                                     + "<Pipeline>Pipeline2</Pipeline>"
                                    + "</Group>"
                                    + "<Group>"
                                     + "<Pipeline>Pipeline3</Pipeline>"
                                     + "<Pipeline>Pipeline4</Pipeline>"
                                    + "</Group>"
                                    + "</WhiteList>"
                                + "<Statuses>"
                                    + "<Passed/>"
                                    + "<Failed/>"
                                    + "<Building/>"
                                + "</Statuses>"
                                + "</Profile>");

            var profileXmlLoader = new ProfileXmlLoader();

            var profile = profileXmlLoader.Load(profileXml, new List<PipelineStatus>{PipelineStatus.Failed});

            Assert.That(profile.Statuses.Count(), Is.EqualTo(1));
            Assert.That(profile.Statuses.First(), Is.EqualTo(PipelineStatus.Failed));
        }
    }
}