using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace GoDashboard.Web.Models
{
    public class CCTrayXmlFeed
    {
        public CCTrayXmlFeed(List<PipelineGroup> pipelineGroups)
        {
            _pipelineGroups = pipelineGroups;
        }
        public string Output {get { return XmlOutput(); }}
        private readonly List<PipelineGroup> _pipelineGroups;

        private string XmlOutput()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            sb.AppendLine("<Projects>");

            foreach (var pipelineGroup in _pipelineGroups)
            {
                sb.AppendLine(ProjectStringFromPipelineGroup(pipelineGroup));
            }

            sb.AppendLine("</Projects>");

            return sb.ToString();
        }

        private static string ProjectStringFromPipelineGroup(PipelineGroup pipelineGroup)
        {
            var firstPipeline = pipelineGroup.Pipelines.FirstOrDefault();
            var projectXml =
                "<Project name=\"{0}\" activity=\"{1}\" lastBuildStatus=\"{2}\" lastBuildLabel=\"{3}\" lastBuildTime=\"{4}\" webUrl=\"{5}\"/>";
            if (firstPipeline != null)
                projectXml = String.Format(projectXml, pipelineGroup.Name,
                                           firstPipeline.Activity ?? (pipelineGroup.Status == "building" ? "Building" : "Sleeping"),
                                           pipelineGroup.Status == "building" ? (pipelineGroup.LastBuildLabelPipeline.LastBuildStatus == "passed" ? "Success" : "Failure") : (pipelineGroup.Status == "passed" ? "Success" : "Failure"),
                                           pipelineGroup.LastBuildLabel, pipelineGroup.LastBuildTime,
                                           pipelineGroup.LastBuildLabelPipeline.WebUrl.Replace("http://", ConfigurationManager.AppSettings["GoUrl"]));
            return projectXml;
        }
    }
}
