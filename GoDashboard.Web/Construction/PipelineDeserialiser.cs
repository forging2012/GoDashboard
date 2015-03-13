using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using GoDashboard.Web.ViewModels;
using GoDashboard.Web.XmlModels;

namespace GoDashboard.Web.Construction
{
    public class PipelineDeserialiser
    {
        private static readonly XmlSerializer Serialiser = new XmlSerializer(typeof(Projects));

        public static List<DisplayablePipeline> DeserialisePipelinesFromXmlAndCleanseContent(XmlDocument xmlDoc)
        {
            var allProjects = Deserialise(xmlDoc);

            var results = new List<DisplayablePipeline>();
            foreach (var project in allProjects.Results)
            {
                var pipelineNameParts = project.Name.Split(new[] { " :: " }, StringSplitOptions.None);
                if (pipelineNameParts.Count() != 2)
                    continue;

                var pipelineName = pipelineNameParts[0];
                var stageName = pipelineNameParts[1];

                if (pipelineName.Contains("Shared-"))
                {
                    if (results.All(p => p.Name != "Shared"))
                        results.Add(new DisplayablePipeline("Shared", ""));
                    var sharedPipeline = results.Find(p => p.Name == "Shared");
                    sharedPipeline.AddStage(CreateStage(pipelineName, project.LastBuildStatus, project.Activity, project.LastBuildTime));
                    continue;
                }


                if (results.All(p => p.Name != pipelineName))
                {
                    var pipeline = new DisplayablePipeline(pipelineName, project.LastBuildLabel);
                    pipeline.Stages.Add(CreateStage(stageName, project.LastBuildStatus, project.Activity, project.LastBuildTime));
                    results.Add(pipeline);
                }
                else
                {
                    var pipeline = results.Find(p => p.Name == pipelineName);
                    pipeline.Stages.Add(CreateStage(stageName, project.LastBuildStatus, project.Activity, project.LastBuildTime));
                }
            }

            return results;
        }

        private static IStage CreateStage(string name, string status, string activity, string lastBuildTime)
        {
            if (status == "Failure")
                return new FailedStage(name, activity, lastBuildTime);
            return new PassedStage(name, activity, lastBuildTime);
        }

        private static Projects Deserialise(XmlDocument xmlDoc)
        {
            var nodeReader = new XmlNodeReader(xmlDoc);
            nodeReader.ReadToDescendant("Projects");
            var result = Serialiser.Deserialize(nodeReader);
            return (Projects)result;
        }
    }
    }
