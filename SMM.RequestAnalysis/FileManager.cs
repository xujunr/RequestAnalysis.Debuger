using System;
using System.IO;
using System.Text;

namespace SMM.RequestAnalysis
{
    public class FileManager
    {
        private const string _defaultPath = @"C:\projects\RequestAnalysis\";
        public void WriteFile(string result)
        {
            if (!Directory.Exists(_defaultPath))
            {
                Directory.CreateDirectory(_defaultPath);
            }
            string path = string.Concat(_defaultPath, DateTime.Now.ToString("yyyyMMdd"), ".txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(result);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
