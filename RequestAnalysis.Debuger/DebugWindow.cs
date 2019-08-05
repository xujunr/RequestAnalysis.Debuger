using System.Diagnostics;

namespace RequestAnalysis.Debuger
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
