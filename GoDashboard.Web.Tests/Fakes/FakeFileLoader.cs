using GoDashboard.Web.Modules.Interfaces;

namespace GoDashboard.Web.Tests.Fakes
{
    public class FakeFileLoader : IFileLoader
    {
        private readonly string _fileContent;

        public FakeFileLoader(string fileContent)
        {
            _fileContent = fileContent;
        }

        public string Load()
        {
            return _fileContent;
        }
    }
}