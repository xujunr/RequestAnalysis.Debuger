using System.Diagnostics;

namespace SMM.RequestAnalysis
{
    public  class DebugWindow
    {
        public void Print(string result)
        {
            Debug.WriteLine(result);
            Debug.Flush();
        }
    }
}
