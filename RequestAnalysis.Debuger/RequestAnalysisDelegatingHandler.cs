using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public class RequestAnalysisDelegatingHandler : DelegatingHandler
    {    
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            new SelfHostRequestAnalysisHandler(request).Excute();
            return base.SendAsync(request, cancellationToken);
        }
    }
}
