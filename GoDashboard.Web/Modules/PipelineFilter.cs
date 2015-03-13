using System.Collections.Generic;
using System.Linq;
using GoDashboard.Web.Models;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Modules
{
    public class PipelineFilter
    {
        private readonly IList<DisplayablePipeline> _displayablePipelines;

        public PipelineFilter(IList<DisplayablePipeline> displayablePipelines)
        {
            _displayablePipelines = displayablePipelines;
        }


        public IList<GroupedDisplayablePipeline> Filter(Profile profile)
        {
            IList<GroupedDisplayablePipeline> groupedDisplayablePipelines = new List<GroupedDisplayablePipeline>();
            if (profile.PipelineGroups.Count > 0)
            {
                foreach (var group in profile.PipelineGroups)
                {
                    var groupedDisplayablePipeline = new GroupedDisplayablePipeline();
                    foreach (var pipeline in group.ProfilePipelines)
                    {
                        var pipeline1 = pipeline;
                        var displayablePipeline = _displayablePipelines.FirstOrDefault(x => x.Name == pipeline1.Name);
                        groupedDisplayablePipeline.Name = group.Name;
                        groupedDisplayablePipeline.ShowName = group.ShowName;
                        if (displayablePipeline != null && profile.Statuses.Contains(displayablePipeline.ActualStatus))
                        {
                            displayablePipeline.HideBuildInfo = pipeline.HideBuildInfo;
                            displayablePipeline.Alias = pipeline.Alias;
                            groupedDisplayablePipeline.Pipelines.Add(displayablePipeline);
                        }
                    }
                    groupedDisplayablePipelines.Add(groupedDisplayablePipeline);
                }
            }
            else
            {
                var groupedDisplayablePipeline = new GroupedDisplayablePipeline { Name = "All", ShowName = false };
                foreach (var displayablePipeline in _displayablePipelines)
                {
                    if (displayablePipeline != null && profile.Statuses.Contains(displayablePipeline.ActualStatus))
                    {
                        groupedDisplayablePipeline.Pipelines.Add(displayablePipeline);
                    }
                }

                groupedDisplayablePipelines.Add(groupedDisplayablePipeline);
            }
            return groupedDisplayablePipelines;
        }
    }
}
