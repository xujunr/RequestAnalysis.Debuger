using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public static partial class Extensions
    {
        private const string HttpRequestMessageKey = "MS_HttpRequestMessage";
        private const string HttpContextKey = "MS_HttpContext";
        private const string SubRouteDataKey = "MS_SubRoutes";
        private const string ActionDescriptorKey = "actions";

        public static HttpRequestMessage GetHttpRequestMessage(this HttpContext context)
            => context.Items[HttpRequestMessageKey] as HttpRequestMessage;
        public static HttpContext GetHttpContext(this HttpRequestMessage request)
            => request.Properties.TryGetValue(HttpContextKey, out object context) ? context as HttpContext : null;
        public static IHttpRouteData[] GetHttpRouteDatas(this RouteValueDictionary values)
          => values[SubRouteDataKey] as IHttpRouteData[];
        public static HttpActionDescriptor[] GetHttpActionDescriptors(this IDictionary<string, object> dataTokens)
          => dataTokens[ActionDescriptorKey] as HttpActionDescriptor[];
    }
}
