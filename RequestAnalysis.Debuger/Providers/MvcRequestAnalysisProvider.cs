using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class MvcRequestAnalysisProvider : RequestAnalysisProvider
    {
        protected override string LookupPrefix => "Controllers";
        protected override string Mode => "Mvc";
        public override RequestAnalysisResult GetRequestAnalysis(RequestAnalysisContext analysisContext)
        {
            RouteData routeData = analysisContext.RouteData;
            RequestAnalysisResult analysisResult = new RequestAnalysisResult();
            string controllerName = routeData.Values.TryGetValue("controller", out object controller) ? controller.ToString() : null;
            analysisResult.ActionName = routeData.Values.TryGetValue("action", out object action) ? action.ToString() : null;

            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(analysisResult.ActionName))
            {
                IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                RequestContext requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
                IController controllerInstance = controllerFactory.CreateController(requestContext, controllerName);
                ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerInstance.GetType());
                ControllerContext controllerContext = new ControllerContext(requestContext, controllerInstance as ControllerBase);
                ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, analysisResult.ActionName);

                analysisResult.Url = HttpContext.Current.Request.Url.ToString();
                analysisResult.Mode = Mode;
                analysisResult.Values = routeData.Values;
                analysisResult.DataTokens = routeData.DataTokens;
                analysisResult.ControllerName = controllerDescriptor.ControllerType.FullName;
                analysisResult.Parameters = actionDescriptor.GetParameters().Select(parameter => parameter.ParameterName).ToArray();

                analysisResult.SupportedHttpMethods = actionDescriptor.GetCustomAttributes(typeof(ActionMethodSelectorAttribute), true)
                    .Select(methodAttribute => methodAttribute.GetType().Name.Replace("Attribute", "").Replace("Http", "")).ToArray();

                analysisResult.SupportedHttpMethods = analysisResult.SupportedHttpMethods.Count() == 0
                    ? new string[1] { "All" } : analysisResult.SupportedHttpMethods;

                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LookupPrefix);
                analysisResult.FilePath = LookupFilePath(path, analysisResult.ControllerName);
            }
            return analysisResult;
        }
    }
}
