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
using System.Web.Http.SelfHost;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class RequestAnalysisModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += OnPostAcquireRequestState;
        }

        private void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            new WebHostRequestAnalysisHandler(new HttpContextWrapper(HttpContext.Current)).Excute();          
        }
        public void Dispose() { }
    }
}
