using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using GoDashboard.Web.Construction;
using GoDashboard.Web.Models;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Modules
{
    public class DashBoard : IDashboard
    {
        private readonly string _content;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _goUrl;

        public DashBoard(string content)
        {
            _content = content;
        }

        public DashBoard(string username, string password)
        {
            _userName = username;
            _password = password;
            _goUrl = ConfigurationManager.AppSettings["GoFeedUrl"];
            _content = GetContent(_goUrl);
        }

        public DashBoard()
        {
            _userName = ConfigurationManager.AppSettings["UserName"];
            _password = ConfigurationManager.AppSettings["Password"];
            _goUrl = ConfigurationManager.AppSettings["GoFeedUrl"];
            _content = GetContent(_goUrl);
        }

        private string UserPass { get { return _userName + ":" + _password; } }

        public XmlDocument GetPipelinesXmlFromServer()
        {
            string content = _content;

            var xDoc = new XmlDocument();
            xDoc.Load(new StringReader(content));
            return xDoc;
        }

        public PipelineGroup GetPipelinesFromServer()
        {
            string content = GetContent(_goUrl);

            var xDoc = new XmlDocument();
            xDoc.Load(new StringReader(content));

            return RetrievePipelinesFromXml(xDoc);
        }

        public List<PipelineGroup> CompressPipelineGroup(PipelineGroup pipelineGroup)
        {
            var relevantPipelineGroupNames = "";

            foreach (var pipeline in
                            pipelineGroup.Pipelines.Where(pipeline => relevantPipelineGroupNames.IndexOf(pipeline.PipelineGroupName, StringComparison.Ordinal) == -1))
            {
                relevantPipelineGroupNames += pipeline.PipelineGroupName + ",";
            }
            relevantPipelineGroupNames = relevantPipelineGroupNames.Substring(0, relevantPipelineGroupNames.Length - 1);

            return relevantPipelineGroupNames.Split(',').Select(relevantPipelineGroupName => CreateCurrentPipelineGroup(pipelineGroup, relevantPipelineGroupName)).ToList();
        }

        public List<DisplayablePipeline> Pipelines()
        {
            var pipelineXml = GetPipelinesXmlFromServer();
            return PipelineDeserialiser.DeserialisePipelinesFromXmlAndCleanseContent(pipelineXml);
        }

        private static PipelineGroup RetrievePipelinesFromXml(XmlDocument xDoc)
        {
            var pipelineGroup = new PipelineGroup();

            foreach (XmlNode xmlNode in xDoc.GetElementsByTagName("Project"))
            {
                pipelineGroup.AddPipeline(xmlNode);
            }

            return pipelineGroup;
        }

        public string GetContent(string url)
        {
            var myReq = WebRequest.Create(url);

            var mycache = new CredentialCache
            {
                  { new Uri(url), "Basic", new NetworkCredential(_userName, _password) }
            };

            myReq.Credentials = mycache;
            myReq.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(UserPass)));

            using (var wr = myReq.GetResponse())
            {
                using (var receiveStream = wr.GetResponseStream())
                {
                    if (receiveStream != null)
                        using (var reader = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                }

                return "Error";
            }
        }

        private PipelineGroup CreateCurrentPipelineGroup(PipelineGroup pipelineGroup, string relevantPipelineGroupName)
        {
            var currentPipelineGroup = new PipelineGroup { Name = relevantPipelineGroupName };
            var relevantPipelines = pipelineGroup.Pipelines.Where(x => x.PipelineGroupName == relevantPipelineGroupName);
            var pipelines = relevantPipelines as Pipeline[] ?? relevantPipelines.ToArray();
            var relevantPipelineStageNames = GetRelevantPipelineStageNames(pipelines);

            return UpdateAllPipelinesStatus(pipelines, relevantPipelineStageNames, currentPipelineGroup);
        }

        private PipelineGroup UpdateAllPipelinesStatus(IEnumerable<Pipeline> relevantPipelines, string relevantPipelineStageNames, PipelineGroup currentPipelineGroup)
        {
            foreach (var relevantPipelineStageName in relevantPipelineStageNames.Split(','))
            {
                var name = relevantPipelineStageName;
                var pipelines = relevantPipelines.Where(x => x.Name.IndexOf(name + " ", StringComparison.Ordinal) > -1).ToList();
                if (pipelines.Count(x => x.Activity == "Building") > 0)
                    pipelines[0].Activity = "Building";
                if (pipelines.Count(x => x.LastBuildStatus == "Failure") > 0)
                    pipelines[0].LastBuildStatus = "Failure";
                currentPipelineGroup.Pipelines.Add(pipelines[0]);
            }
            return currentPipelineGroup;
        }

        private string GetRelevantPipelineStageNames(IEnumerable<Pipeline> relevantPipelines)
        {
            var relevantPipelineStageNames = "";
            foreach (var relevantPipeline in relevantPipelines)
            {
                if (relevantPipelineStageNames.IndexOf(relevantPipeline.Name.Substring(0, relevantPipeline.Name.IndexOf(" ::")), StringComparison.Ordinal) == -1)
                    relevantPipelineStageNames += relevantPipeline.Name.Substring(0, relevantPipeline.Name.IndexOf(" ::", StringComparison.Ordinal)) + ",";
            }
            relevantPipelineStageNames = relevantPipelineStageNames.Substring(0,
                                                                              relevantPipelineStageNames.Length - 1);
            return relevantPipelineStageNames;
        }
    }
}