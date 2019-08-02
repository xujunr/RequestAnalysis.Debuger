using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class WebApiRequestAnalysisFactory : RequestAnalysisFactory
    {
        protected override string LookupPrefix => "Api";
        protected override string Mode => "Web API";
        public override RequestAnalysis CreateRequestAnalysis(RouteData routeData)
        {
            RequestAnalysis requestAnalysis = new RequestAnalysis();
            HttpRequestMessage requestMessage = HttpContext.Current.GetHttpRequestMessage();

            IHttpRouteData httpRouteData = routeData.Values.GetHttpRouteDatas().FirstOrDefault();
            HttpActionDescriptor actionDescriptor = httpRouteData.Route.DataTokens.GetHttpActionDescriptors().FirstOrDefault();

            requestAnalysis.Url = requestMessage.RequestUri.ToString();
            requestAnalysis.SupportedHttpMethods = actionDescriptor.SupportedHttpMethods.Select(method => method.Method).ToArray();
            requestAnalysis.Parameters = actionDescriptor.GetParameters().Select(parameter => parameter.ParameterName).ToArray();
            requestAnalysis.ActionName = actionDescriptor.ActionName;
            requestAnalysis.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            requestAnalysis.Values = routeData.Values;
            requestAnalysis.DataTokens = httpRouteData.Route.DataTokens as RouteValueDictionary;
            requestAnalysis.Mode = Mode;

            string path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, LookupPrefix);
            requestAnalysis.FilePath = LookupFilePath(path, requestAnalysis.ControllerName);

            return requestAnalysis;
        }
    }
}
