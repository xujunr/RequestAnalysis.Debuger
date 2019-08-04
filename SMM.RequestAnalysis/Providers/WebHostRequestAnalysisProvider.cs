using System;
using System.Collections.Generic;
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
    public  class WebHostRequestAnalysisProvider : IRequestAnalysisProvider
    {
        private MvcRequestAnalysisProvider _mvcProvider;
        private HttpRequestAnalysisProvider _httpProvider;
        public WebHostRequestAnalysisProvider(
            MvcRequestAnalysisProvider mvcProvider, 
            HttpRequestAnalysisProvider httpProvider)
        {
            this._mvcProvider = mvcProvider;
            this._httpProvider = httpProvider;
        }
      
        public RequestAnalysis GetRequestAnalysis(RequestAnalysisContext requestAnalysisContext)
        {
            if (requestAnalysisContext.RouteData != null)
            {
                if (requestAnalysisContext.RouteData?.RouteHandler.GetType() == typeof(HttpControllerRouteHandler))
                {
                    return _httpProvider.GetRequestAnalysis(requestAnalysisContext);
                }
                return _mvcProvider.GetRequestAnalysis(requestAnalysisContext);
            }
            return null;
        }
    }
}
