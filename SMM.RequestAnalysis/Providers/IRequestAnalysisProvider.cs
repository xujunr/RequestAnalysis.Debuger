using System.Web;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public interface IRequestAnalysisProvider
    {
         RequestAnalysis GetRequestAnalysis(RequestAnalysisContext requestAnalysisContext);
    }
}
