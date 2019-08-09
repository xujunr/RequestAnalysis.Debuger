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

namespace RequestAnalysis.Debuger
{
    public class HttpRequestAnalysisProvider : RequestAnalysisProvider
    {
        protected override string LookupPrefix => "Api";
        protected override string Mode => "Http";
        public override RequestAnalysisResult GetRequestAnalysis(RequestAnalysisContext analysisContext)
        {
            HttpRequestMessage request = analysisContext.RequestMessage;
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


            RequestAnalysisResult analysisResult = new RequestAnalysisResult();
            analysisResult.Url = request.RequestUri.ToString();
            analysisResult.SupportedHttpMethods = actionDescriptor.SupportedHttpMethods.Select(method => method.Method).ToArray();
            analysisResult.Parameters = actionDescriptor.GetParameters().Select(parameter => parameter.ParameterName).ToArray();
            analysisResult.ActionName = actionDescriptor.ActionName;
            analysisResult.ControllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            analysisResult.Values = httpRouteData.Values;
            analysisResult.DataTokens = httpRouteData.Route.DataTokens;
            analysisResult.Mode = Mode;

            string path = new DirectoryInfo(string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, LookupPrefix)).FullName;
            analysisResult.FilePath = LookupFilePath(path, analysisResult.ControllerName);

            return analysisResult;
        }
    }
}
