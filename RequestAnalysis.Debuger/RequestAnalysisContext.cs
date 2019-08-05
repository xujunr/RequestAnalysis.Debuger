using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class RequestAnalysisContext
    {
        public RequestAnalysisContext(RouteData routeData, HttpContextWrapper httpContext)
        {
            this.RouteData = routeData;
            this.HttpContext = httpContext;
        }
        public RequestAnalysisContext(IHttpRouteData httpRouteData, HttpRequestMessage requestMessage)
        {
            this.HttpRouteData = httpRouteData;
            this.RequestMessage = requestMessage;
        }

        public RequestAnalysisContext(
            RouteData routeData,
            HttpContextWrapper httpContext,
            HttpRequestMessage requestMessage) : this(routeData, httpContext)
        {
            this.RequestMessage = requestMessage;
        }
        public IHttpRouteData HttpRouteData { get; set; }
        public RouteData RouteData { get; set; }
        public HttpContextWrapper HttpContext { get; set; }
        public HttpRequestMessage RequestMessage { get; set; }
    }
}
