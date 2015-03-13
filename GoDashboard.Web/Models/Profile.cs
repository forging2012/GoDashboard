using System.Collections.Generic;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Models
{
    public class Profile
    {
        public Profile()
        {
            Statuses = new List<PipelineStatus> { PipelineStatus.Passed, PipelineStatus.Failed, PipelineStatus.Building };
        }
        public IEnumerable<PipelineStatus> Statuses;
        public bool ShowPassedCount;

        public IList<GroupedPipeline> PipelineGroups = new List<GroupedPipeline>();

    }
}