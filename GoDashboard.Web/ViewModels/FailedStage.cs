using System;

namespace GoDashboard.Web.ViewModels
{
    public class FailedStage : IStage
    {
        public FailedStage(string name, string activity, string lastBuildTime)
        {
            Name = name;
            Activity = activity;
            LastBuildTime = DateTime.Parse(lastBuildTime);
        }

        public string Name { get; private set; }
        public string Status { get { return "failed"; } }
        public string Activity { get; private set; }
        public DateTime LastBuildTime { get; private set; }
    }
}