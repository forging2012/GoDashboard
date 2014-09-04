using System;
using System.Collections.Generic;
using System.Linq;
using GoDashboard.Web.Models;

namespace GoDashboard.Web.ViewModels
{
    public class DisplayablePipeline
    {
        private bool _greyStatus;

        public DisplayablePipeline(string name, string lastBuildLabel)
        {
            HideBuildInfo = false;
            Name = name;
            LastBuildLabel = lastBuildLabel;
            Stages = new List<IStage>();
        }

        public void AddStage(IStage stage)
        {
            Stages.Add(stage);
        }

        public string Name { get; private set; }

        public string Alias { get; set; }

        public string DisplayName 
        { 
            get
            {
                return Alias ?? Name;
            }
        }

        public string Status
        {
            get { return GetStatus(); }
            set
            {
                _greyStatus = value == "grey";
            }
        }
        public string FixOverdue { get { return GetFixOverdue(); } }
        public string LastBuildLabel { get; private set; }
        public DateTime LastBuildTime { get { return Stages.Max(s => s.LastBuildTime); } }
        public List<IStage> Stages { get; set; }

        public bool HideBuildInfo { get; set; }

        public PipelineStatus ActualStatus
        {
            get
            {
                if (Status == "failed")
                {
                    return PipelineStatus.Failed;
                }
                return Status == "building" ? PipelineStatus.Building : PipelineStatus.Passed;
            }
        }

        private string GetStatus()
        {
            if (Stages.Any(s => s.Activity == "Building"))
            {
                return "building";
            }
            if (Stages.Any(s => s.Status == "failed"))
            {
                return _greyStatus ? "grey" : "failed";
            }
            return "passed";
        }

        private string GetFixOverdue()
        {
            return Stages.Any(s => s.Status == "failed") ? "overdue" : "good";
        }
    }
}
