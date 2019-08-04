using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public class CompositeRequestAnalysisProvider : IRequestAnalysisProvider
    {
        public IList<IRequestAnalysisProvider> Providers { get; set; }
        public CompositeRequestAnalysisProvider(IList<IRequestAnalysisProvider> providers)
        {
            Providers = providers;
        }
        public CompositeRequestAnalysisProvider()
        {
            Providers = new List<IRequestAnalysisProvider>();
            Providers.Add(new WebHostRequestAnalysisProvider(
                new MvcRequestAnalysisProvider(),
                new HttpRequestAnalysisProvider())
           );
        }
        public RequestAnalysis GetRequestAnalysis(RequestAnalysisContext requestAnalysisContext)
        {
            foreach (var provider in Providers)
            {
                RequestAnalysis requestAnalysis = provider.GetRequestAnalysis(requestAnalysisContext);
                if (requestAnalysis != null)
                {
                    return requestAnalysis;
                }
            }
            return null;
        }
    }
}
