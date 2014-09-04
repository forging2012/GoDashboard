using System;

namespace GoDashboard.Web.ViewModels
{
    public class PassedStage : IStage
    {
        public PassedStage(string name, string activity, string lastBuildTime)
        {
            Name = name;
            Activity = activity;
            LastBuildTime = DateTime.Parse(lastBuildTime);
        }

        public string Name { get; private set; }
        public string Status { get { return "passed"; } }
        public string Activity { get; private set; }
        public DateTime LastBuildTime { get; private set; }
    }
}