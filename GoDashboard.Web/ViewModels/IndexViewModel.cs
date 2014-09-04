using System.Collections.Generic;

namespace GoDashboard.Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<DisplayablePipeline> Pipelines;
        public int PassedCount;
        public bool ShowPassedCount;
        public IList<GroupedDisplayablePipeline> Groups;
    }
}