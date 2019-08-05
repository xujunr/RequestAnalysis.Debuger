using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace RequestAnalysis.Debuger
{
    public class SelfHostRequestAnalysisHandler : RequestAnalysisHandler
    {
        public HttpRequestMessage _request { get; set; }
        public SelfHostRequestAnalysisHandler(HttpRequestMessage request) => _request = request;

        public override void Excute()
        {
            IHttpRouteData httpRouteData = _request.GetConfiguration().Routes.GetRouteData(_request);
            if (httpRouteData != null)
            {
                List<IRequestAnalysisProvider> providers = new List<IRequestAnalysisProvider> { new SelfHostHttpRequestAnalysisProvider() };
                CompositeRequestAnalysisProvider compositeProvider = new CompositeRequestAnalysisProvider(providers);
                base.ExcuteCore( new RequestAnalysisContext(httpRouteData, _request),compositeProvider);
            }
        }
    }
}
