using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class RequestAnalysisModule : IHttpModule
    {
        private readonly bool _isEnableFileOutput = true;
        private Action<string> Output;
        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += OnPostAcquireRequestState;

            if (HttpContext.Current.IsDebuggingEnabled)
            {
                Output += new DebugWindowRequestAnalysis().OutputDebugWindow;
            }
            if (_isEnableFileOutput)
            {
                Output += new FileRequestAnalysis().WriteFile;
            }
        }

        private void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            RequestAnalysis requestAnalysis=null;
            if (routeData != null)
            {
                if (routeData.Values.ContainsKey("MS_SubRoutes"))
                {
                    requestAnalysis=new WebApiRequestAnalysisFactory().CreateRequestAnalysis(routeData);
                }
                else
                {
                    requestAnalysis = new MVCRequestAnalysisFactory().CreateRequestAnalysis(routeData);
                }
            }

            if (!string.IsNullOrEmpty(requestAnalysis?.Mode))
            {
                OutputResult(requestAnalysis);
            }
        }
        private void OutputResult(RequestAnalysis requestAnalysis)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=================================================================================");
            sb.AppendLine($"Time: {DateTime.Now.ToLocalTime()}");
            sb.AppendLine($"Url: {requestAnalysis.Url}");
            sb.AppendLine($"Mode: {requestAnalysis.Mode}");
            sb.AppendLine($"Controller: {requestAnalysis.ControllerName}");
            sb.AppendLine($"Action: {requestAnalysis.ActionName}");
            sb.AppendLine($"FilePath: {requestAnalysis.FilePath}");
            sb.AppendLine($"Parameters: {string.Join(",", requestAnalysis.Parameters)}");
            sb.AppendLine($"SupportedHttpMethods: {string.Join(",", requestAnalysis.SupportedHttpMethods)}");
            sb.AppendLine("=================================================================================");

            Output(sb.ToString());
        }

        public void Dispose() { }
    }
}
