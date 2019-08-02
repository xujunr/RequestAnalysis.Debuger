using System.Diagnostics;

namespace SMM.RequestAnalysis
{
    public  class DebugWindowRequestAnalysis
    {
        public void OutputDebugWindow(string result)
        {
            Debug.WriteLine(result);
            Debug.Flush();
        }
    }
}
