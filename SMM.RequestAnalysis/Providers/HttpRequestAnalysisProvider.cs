using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class HttpRequestAnalysisProvider : RequestAnalysisProvider
    {
        protected override string LookupPrefix => "Api";
        protected override string Mode => "Http";
        public override RequestAnalysis GetRequestAnalysis(RequestAnalysisContext requestAnalysisContext)
        {
            HttpRequestMessage request = requestAnalysisContext.RequestMessage;
            HttpConfiguration httpConfiguration = request.GetConfiguration();
            IHttpRouteData httpRouteData = httpConfiguration.Routes.GetRouteData(request);
            request.SetRouteData(httpRouteData);
            //IAssembliesResolver assembliesResolver= httpConfiguration.Services.GetAssembliesResolver();
            //IHttpControllerTypeResolver controllerTypeResolver = httpConfiguration.Services.GetHttpControllerTypeResolver();
            //ICollection<Type> controllerTypes= controllerTypeResolver.GetControllerTypes(assembliesResolver);
            IHttpControllerSelector controllerSelector = httpConfiguration.Services.GetHttpControllerSelector();
            HttpControllerDescriptor controllerDescriptor = controllerSelector.SelectController(request);

            HttpControllerContext controllerContext = new HttpControllerContext(httpConfiguration, httpRouteData, request);
            controllerContext.ControllerDescriptor = controllerDescriptor;
            IHttpActionSelector actionSelector = httpConfiguration.Services.GetActionSelector();
            HttpActionDescriptor actionDescriptor = actionSelector.SelectAction(controllerContext);


            RequestAnalysis requestAnalysis = new RequestAnalysis();
            requestAnalysis.Url = request.RequestUri.ToString();
            requestAnalysis.SupportedHttpMethods = actionDescriptor.SupportedHttpMethods.Select(method => method.Method).ToArray();
            requestAnalysis.Parameters = actionDescriptor.GetParameters().Select(parameter => parameter.ParameterName).ToArray();
            requestAnalysis.ActionName = actionDescriptor.ActionName;
            requestAnalysis.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            requestAnalysis.Values = httpRouteData.Values;
            requestAnalysis.DataTokens = httpRouteData.Route.DataTokens;
            requestAnalysis.Mode = Mode;

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LookupPrefix);
            requestAnalysis.FilePath = LookupFilePath(path, requestAnalysis.ControllerName);

            return requestAnalysis;
        }
    }
}
