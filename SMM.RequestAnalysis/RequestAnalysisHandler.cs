using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace SMM.RequestAnalysis
{
    public abstract class RequestAnalysisHandler
    {
        private readonly bool _isEnableFileOutput = true;
        private Action<string> Output;
        public RequestAnalysisHandler()
        {
            SetDebugWindowRequestAnalysis();

            if (_isEnableFileOutput)
            {
                Output += new FileManager().WriteFile;
            }
        }
        public abstract  void Excute();
    
        protected void ExcuteCore(RequestAnalysisContext requestAnalysisContext, IRequestAnalysisProvider provider)
        {
            try
            {
                RequestAnalysis requestAnalysis = provider.GetRequestAnalysis(requestAnalysisContext);
                if (!string.IsNullOrEmpty(requestAnalysis?.Mode))
                {
                    OutputResult(requestAnalysis);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Url: {HttpContext.Current.Request.Url}");
                Debug.WriteLine(ex.Message);
            }

        }


        [Conditional("DEBUG")]
        private void SetDebugWindowRequestAnalysis()
        {
            Output += new DebugWindow().Print;
        }

        protected virtual void OutputResult(RequestAnalysis requestAnalysis)
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
    }
}
