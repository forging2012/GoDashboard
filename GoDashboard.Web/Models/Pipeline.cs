namespace GoDashboard.Web.Models
{
    using System;
    using System.Globalization;


    public class Pipeline
    {
        public string Name { get; set; }
        public string ActualName { get { return Name.Substring(0, Name.IndexOf(" ::", StringComparison.Ordinal)); } }
        public string Activity { get; set; }
        public string LastBuildStatus { get; set; }
        public string LastBuildLabel { get; set; }
        public int LastBuildLabelInt
        {
            get
            {
                var intString = LastBuildLabel.Substring(0,
                                                            LastBuildLabel.IndexOf(" ", StringComparison.Ordinal) == -1
                                                                ? LastBuildLabel.Length
                                                                : LastBuildLabel.IndexOf(" ", StringComparison.Ordinal));
                int lastBuildLabel;
                int.TryParse(intString, out lastBuildLabel);
                return lastBuildLabel;
            }
        }
        public string LastBuildTime
        {
            get
            {
                return LastBuildTimeDateTime.ToString(CultureInfo.InvariantCulture);
            }
        }
        public DateTime LastBuildTimeDateTime { get; set; }

        public string WebUrl { get; set; }
        public string Status
        {
            get
            {
                return LastBuildStatus == "Failure" ? "failed" : "passed";
            }
        }
        public string PipelineGroupName
        {
            get { return Name.Split('-')[0]; }
        }
    }
}