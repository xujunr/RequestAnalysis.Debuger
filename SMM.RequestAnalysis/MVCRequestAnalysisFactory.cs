using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class MVCRequestAnalysisFactory : RequestAnalysisFactory
    {
        protected override string LookupPrefix => "Controllers";
        protected override string Mode => "MVC Test";
        public override RequestAnalysis CreateRequestAnalysis(RouteData routeData)
        {
            RequestAnalysis requestAnalysis = new RequestAnalysis();
            string controllerName = routeData.Values.TryGetValue("controller", out object controller) ? controller.ToString() : null;
            requestAnalysis.ActionName = routeData.Values.TryGetValue("action", out object action) ? action.ToString() : null;

            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(requestAnalysis.ActionName))
            {
                IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                RequestContext requestContext = new RequestContext(new HttpContextWrapper(HttpContext.Current), routeData);
                IController controllerInstance = controllerFactory.CreateController(requestContext, controllerName);
                ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controllerInstance.GetType());
                ControllerContext controllerContext = new ControllerContext(requestContext, controllerInstance as ControllerBase);
                ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controllerContext, requestAnalysis.ActionName);

                requestAnalysis.Url = HttpContext.Current.Request.Url.ToString();
                requestAnalysis.Mode = Mode;
                requestAnalysis.Values = routeData.Values;
                requestAnalysis.DataTokens = routeData.DataTokens;
                requestAnalysis.ControllerName = controllerDescriptor.ControllerType.FullName;
                requestAnalysis.Parameters = actionDescriptor.GetParameters().Select(parameter => parameter.ParameterName).ToArray();

                requestAnalysis.SupportedHttpMethods = actionDescriptor.GetCustomAttributes(typeof(ActionMethodSelectorAttribute), true)
                    .Select(methodAttribute => methodAttribute.GetType().Name.Replace("Attribute", "").Replace("Http", "")).ToArray();

                requestAnalysis.SupportedHttpMethods = requestAnalysis.SupportedHttpMethods.Count() == 0
                    ? new string[1] { "All" } : requestAnalysis.SupportedHttpMethods;

                string path = string.Concat(AppDomain.CurrentDomain.BaseDirectory, LookupPrefix);
                requestAnalysis.FilePath = LookupFilePath(path, requestAnalysis.ControllerName);
            }
            return requestAnalysis;
        }
    }
}
