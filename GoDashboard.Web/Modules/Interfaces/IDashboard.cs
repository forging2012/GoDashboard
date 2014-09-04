using System.Collections.Generic;
using System.Xml;
using GoDashboard.Web.Models;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Modules.Interfaces
{
    public interface IDashboard
    {
        PipelineGroup GetPipelinesFromServer();
        XmlDocument GetPipelinesXmlFromServer();
        List<PipelineGroup> CompressPipelineGroup(PipelineGroup pipelineGroup);
        List<DisplayablePipeline> Pipelines();
    }
}