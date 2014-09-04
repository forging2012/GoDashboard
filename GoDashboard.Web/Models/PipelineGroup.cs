using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace GoDashboard.Web.Models
{
    public class PipelineGroup
    {
        public PipelineGroup()
        {
            Pipelines = new List<Pipeline>();
        }
        public List<Pipeline> Pipelines;
        public string Name;

        public string FriendlyName
        {
            get { return Name.Replace(".", ""); }
        }

        public string Status
        {
            get
            {
                if (Pipelines.Count(x => x.Status == "failed") > 0)
                {
                    return "failed";
                }
                return "passed";
            }
        }

        public string LastBuildLabel
        {
            get
            {
                if (ConfigurationManager.AppSettings[Name + "Label"] == null)
                    return LastBuildLabelPipeline.LastBuildLabelInt.ToString();

                return Pipelines.First(x => x.ActualName.IndexOf(ConfigurationManager.AppSettings[Name + "Label"], StringComparison.Ordinal) >= 0).LastBuildLabel + "(" + LastBuildLabelPipeline.LastBuildLabelInt + ")";
            }
        }

        public Pipeline LastBuildLabelPipeline
        {
            get
            {
                return Pipelines.Count(x => x.Status == "failed") > 0 ? 
                    Pipelines.Where(x => x.Status == "failed").OrderBy(x => x.LastBuildLabelInt).ToList().First() : 
                    Pipelines.OrderByDescending(x => x.LastBuildLabelInt).ToList().First();
            }
        }

        public DateTime LastBuildTimeDateTime
        {
            get
            {
                return Pipelines.Count(x => x.Status == "failed") > 0 ? 
                    Pipelines.Where(x => x.Status == "failed").OrderByDescending(x => x.LastBuildTimeDateTime).ToList().First().LastBuildTimeDateTime : 
                    Pipelines.OrderByDescending(x => x.LastBuildTimeDateTime).ToList().First().LastBuildTimeDateTime;
            }
        }

        public string LastBuildTime
        {
            get
            {
                return LastBuildTimeDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }

        public string FixOverdue
        {
            get
            {
                return Pipelines.Count(x => x.Status == "failed" && x.LastBuildTimeDateTime.AddMinutes(30) < DateTime.Now) > 0 ? "overdue" : "good";
            }
        }

        public void AddPipeline(XmlNode xmlNode)
        {
            if (xmlNode.Attributes != null)
            {
                var pipelineName = xmlNode.Attributes["name"].Value;
                if (pipelineName.IndexOf("::", StringComparison.Ordinal) == pipelineName.LastIndexOf("::", StringComparison.Ordinal))
                {
                    DateTime lastBuildTimeDateTime;
                    DateTime.TryParse(xmlNode.Attributes["lastBuildTime"].Value, out lastBuildTimeDateTime);

                    var pipeline = new Pipeline
                    {
                        Name = pipelineName,
                        Activity = xmlNode.Attributes["activity"].Value,
                        LastBuildStatus = xmlNode.Attributes["lastBuildStatus"].Value,
                        LastBuildLabel = xmlNode.Attributes["lastBuildLabel"].Value,
                        LastBuildTimeDateTime = lastBuildTimeDateTime,
                        WebUrl = xmlNode.Attributes["webUrl"].Value
                    };
                    Pipelines.Add(pipeline);
                }
            }
        }
    }
}