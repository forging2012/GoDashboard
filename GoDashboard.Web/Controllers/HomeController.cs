using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoDashboard.Web.Models;
using GoDashboard.Web.Modules;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IDashboard _dashboard;
        private readonly IXmlProfileRetriever _xmlProfileRetriever;

        public HomeController()
            : this(new DashBoard(), new XmlProfileRetriever(new XmlFileLoader("~/Xml/Profiles.xml")))
        {
        }

        public HomeController(IDashboard dashboard, IXmlProfileRetriever xmlProfileRetriever)
        {
            _xmlProfileRetriever = xmlProfileRetriever;
            _dashboard = dashboard;
        }

        public ActionResult Index(string profileName, string status = "")
        {
            IndexViewModel model = GetViewModel(profileName, status);
            return View("Index", model);
        }

        public ActionResult Refresh(string profileName, string status = "")
        {
            IndexViewModel model = GetViewModel(profileName, status);
            return View("Refresh", model);
        }

        private IndexViewModel GetViewModel(string profileName, string status)
        {
            var pipelines = _dashboard.Pipelines();

            var passedCount = pipelines.Count(p => p.ActualStatus == PipelineStatus.Passed);
            var showPassedCount = false;
            IList<GroupedDisplayablePipeline> groups = new List<GroupedDisplayablePipeline>();

            var statuses = new List<PipelineStatus>();

            if (!string.IsNullOrEmpty(status))
            {
                statuses =
                    status.Split(',')
                          .Select(ParseStatus)
                          .ToList();
            }

            if (!string.IsNullOrEmpty(profileName))
            {
                var profileXml = _xmlProfileRetriever.GetProfileXml(profileName);
                var profile = new ProfileXmlLoader().Load(profileXml, statuses);
                var pipelineFilter = new PipelineFilter(pipelines);
                groups = pipelineFilter.Filter(profile);
                showPassedCount = profile.ShowPassedCount;
            }

            return new IndexViewModel
            {
                Pipelines = pipelines,
                PassedCount = passedCount,
                Groups = groups,
                ShowPassedCount = showPassedCount
            };
        }

        private static PipelineStatus ParseStatus(string status)
        {
            try
            {
                return (PipelineStatus)Enum.Parse(typeof(PipelineStatus), status, true);
            }
            catch (ArgumentException)
            {
                return PipelineStatus.Unspecified;
            }
        }
    }
}
