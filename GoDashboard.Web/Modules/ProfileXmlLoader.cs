using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using GoDashboard.Web.Models;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Modules
{
    public class ProfileXmlLoader
    {
        public Profile Load(XElement profilesXml, List<PipelineStatus> pipelineStatuses)
        {
            var profile = new Profile();

            if (profilesXml == null)
            {
                return profile;
            }

            var xElement = profilesXml.Element("WhiteList");
            if (xElement != null)
            {
                var groupNodes = xElement.Elements("Group");

                var i = 0;
                foreach (var groupNode in groupNodes)
                {
                    var xAttribute = groupNode.Attribute("name");
                    var groupName = xAttribute != null ? xAttribute.Value : i++.ToString(CultureInfo.InvariantCulture);

                    xAttribute = groupNode.Attribute("showName");
                    var showName = true;
                    if (xAttribute != null)
                    {
                        bool.TryParse(xAttribute.Value, out showName);
                    }

                    var groupPipelineNodes = groupNode.Elements("Pipeline");

                    var profilePipelines = LoadProfilePipelines(groupPipelineNodes);

                    var groupedPipeline = new GroupedPipeline { Name = groupName, ProfilePipelines = profilePipelines, ShowName = showName };
                    profile.PipelineGroups.Add(groupedPipeline);
                }
            }

            var statusList = new List<PipelineStatus>();

            if (pipelineStatuses.Any())
            {
                statusList = pipelineStatuses;
            }
            else
            {
                var statusXml = profilesXml.Element("Statuses");
                if (statusXml.HasElements)
                {
                    var statusElements = statusXml.Elements();
                    foreach (var statusElement in statusElements)
                    {
                        if (statusElement.Name == "Failed")
                            statusList.Add(PipelineStatus.Failed);
                        if (statusElement.Name == "Passed")
                            statusList.Add(PipelineStatus.Passed);
                        if (statusElement.Name == "Building")
                            statusList.Add(PipelineStatus.Building);
                    }
                }
            }

            profile.Statuses = statusList;

            bool showPassedCount;
            bool.TryParse((profilesXml.Element("ShowPassedCount") ?? XElement.Parse("<ShowPassedCount>false</ShowPassedCount>")).Value, out showPassedCount);

            profile.ShowPassedCount = showPassedCount;

            return profile;
        }

        private static List<ProfilePipeline> LoadProfilePipelines(IEnumerable<XElement> groupPipelineNodes)
        {
            var profilePipelines = new List<ProfilePipeline>();

            foreach (var groupPipelineNode in groupPipelineNodes)
            {
                var profilePipeline = new ProfilePipeline();
                bool hideBuildInfo = false;

                var hideBuildInfoAttribute = groupPipelineNode.Attribute("hideBuildInfo");
                if (hideBuildInfoAttribute != null)
                {
                    bool.TryParse(hideBuildInfoAttribute.Value, out hideBuildInfo);
                }

                profilePipeline.HideBuildInfo = hideBuildInfo;

                var aliasAttribute = groupPipelineNode.Attribute("alias");
                if (aliasAttribute != null)
                {
                    profilePipeline.Alias = aliasAttribute.Value;
                }

                profilePipeline.Name = groupPipelineNode.Value;

                profilePipelines.Add(profilePipeline);
            }
            return profilePipelines;
        }

        public Profile Load(XElement profilesXml)
        {
            return Load(profilesXml, new List<PipelineStatus>());
        }
    }
}