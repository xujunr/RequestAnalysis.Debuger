using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class WebHostRequestAnalysisHandler : RequestAnalysisHandler
    {
        public HttpContextWrapper _contextWrapper { get; set; }
        public WebHostRequestAnalysisHandler(HttpContextWrapper contextWrapper)
            => _contextWrapper = contextWrapper;
        public override void Excute()
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(_contextWrapper);
            HttpRequestMessage request = _contextWrapper.GetHttpRequestMessage();
            RequestAnalysisContext analysisContext = new RequestAnalysisContext(routeData, _contextWrapper, request);

            if (routeData != null)
            {
                base.ExcuteCore(analysisContext, new CompositeRequestAnalysisProvider());
            }
        }
    }
}
