using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public abstract class RequestAnalysisProvider : IRequestAnalysisProvider
    {
        public abstract RequestAnalysisResult GetRequestAnalysis(RequestAnalysisContext analysisContext);
        protected virtual string Mode { get; }
        protected virtual string LookupPrefix { get; }

        protected virtual string LookupFilePath(string path, string controllerName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            string[] nameSpaceSplits = controllerName.Split('.');

            if (nameSpaceSplits.Length < 2 || path.Contains(nameSpaceSplits[nameSpaceSplits.Length - 2]))
            {
                foreach (var file in dirInfo.GetFiles())
                {
                    if (file.Name.Contains(controllerName) || controllerName.Contains(file.Name.Substring(0, file.Name.Length - ".cs".Length)))
                    {
                        return file.FullName;
                    }
                }
            }

            foreach (var directory in dirInfo.GetDirectories())
            {
                return LookupFilePath(directory.FullName, controllerName);
            }

            return string.Empty;
        }
    }
}
