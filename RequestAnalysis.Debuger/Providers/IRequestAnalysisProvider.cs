using System.Web;
using System.Web.Routing;

namespace RequestAnalysis.Debuger
{
    public interface IRequestAnalysisProvider
    {
         RequestAnalysisResult GetRequestAnalysis(RequestAnalysisContext analysisContext);
    }
}
