using System.Collections.Generic;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class RequestAnalysisResult
    {
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Mode { get; set; }
        public string FilePath { get; set; }
        public string[] Parameters { get; set; }
        public string[] SupportedHttpMethods { get; set; }
        public IDictionary<string,object> Values { get; set; }
        public IDictionary<string, object> DataTokens { get; set; }
    }
}
