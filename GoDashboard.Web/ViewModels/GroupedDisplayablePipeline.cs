using System.Collections.Generic;

namespace GoDashboard.Web.ViewModels
{
    public class GroupedDisplayablePipeline
    {
        public string Name { get; set; }

        public bool ShowName { get; set; }

        public List<DisplayablePipeline> Pipelines = new List<DisplayablePipeline>();
    }
}