using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class RequestAnalysis
    {
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Mode { get; set; }
        public string FilePath { get; set; }
        public string[] Parameters { get; set; }
        public string[] SupportedHttpMethods { get; set; }
        public RouteValueDictionary Values { get; set; }
        public RouteValueDictionary DataTokens { get; set; }
    }
}
