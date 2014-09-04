using System.Collections.Generic;

namespace GoDashboard.Web.ViewModels
{
    public class GroupedPipeline
    {
        public string Name { get; set; }

        public List<ProfilePipeline> ProfilePipelines { get; set; }

        public bool ShowName { get; set; }
    }
}
