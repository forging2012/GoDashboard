namespace GoDashboard.Web.ViewModels
{
    public class PipelineInfo
    {
        public PipelineInfo(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}