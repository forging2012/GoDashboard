using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoDashboard.Web.Models;
using GoDashboard.Web.Modules;
using GoDashboard.Web.Modules.Interfaces;
using GoDashboard.Web.ViewModels;

namespace GoDashboard.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IDashboard _dashboard;
        private readonly IXmlProfileRetriever _xmlProfileRetriever;

        public HomeController() : this(new DashBoard(), new XmlProfileRetriever(new XmlFileLoader("~/Xml/Profiles.xml")))
        {
        }

        public HomeController(IDashboard dashboard, IXmlProfileRetriever xmlProfileRetriever)
        {
            _xmlProfileRetriever = xmlProfileRetriever;
            _dashboard = dashboard;
        }

        public ActionResult Index(string profileName)
        {
            IndexViewModel model = GetViewModel(profileName);
            return View("Index", model);
        }

        public ActionResult Refresh(string profileName)
        {
            IndexViewModel model = GetViewModel(profileName);
            return View("Refresh", model);
        }

        private IndexViewModel GetViewModel(string profileName)
        {
            List<DisplayablePipeline> pipelines = _dashboard.Pipelines();
            
            var passedCount = pipelines.Count(p => p.ActualStatus == PipelineStatus.Passed);
            var showPassedCount = false;
            IList<GroupedDisplayablePipeline> groups = new List<GroupedDisplayablePipeline>();

            if (!string.IsNullOrEmpty(profileName))
            {
                var profileXml = _xmlProfileRetriever.GetProfileXml(profileName);
                var profile = new ProfileXmlLoader().Load(profileXml);
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
    }
}