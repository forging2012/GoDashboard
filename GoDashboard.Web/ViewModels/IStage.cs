using System;

namespace GoDashboard.Web.ViewModels
{
    public interface IStage
    {
        string Name { get; }
        string Status { get; }
        string Activity { get; }
        DateTime LastBuildTime { get; }
    }
}