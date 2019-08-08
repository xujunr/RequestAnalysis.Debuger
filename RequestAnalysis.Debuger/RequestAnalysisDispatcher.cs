using System;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace RequestAnalysis.Debuger
{
    public abstract class RequestAnalysisDispatcher
    {
        private readonly bool _isEnableFileOutput = true;
        private Action<string> Output;
        public RequestAnalysisDispatcher()
        {
            SetDebugWindowRequestAnalysis();

            if (_isEnableFileOutput)
            {
                Output += new FileOperation().WriteFile;
            }
        }
        public abstract  void Excute();
    
        protected void ExcuteCore(RequestAnalysisContext analysisContext, IRequestAnalysisProvider provider)
        {
            try
            {
                RequestAnalysisResult analysisResult = provider.GetRequestAnalysis(analysisContext);
                if (!string.IsNullOrEmpty(analysisResult?.Mode))
                {
                    OutputResult(analysisResult);
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

        protected virtual void OutputResult(RequestAnalysisResult requestAnalysis)
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
