using System.Diagnostics;
using System.IO;

namespace TDSPaster
{
    class Util
    {
        //Opens the file in sublime text. Path to your sublime text is hardcoded. 
        public static void OpenInSublime(string fileLocation)
        {
            ProcessStartInfo pi = new ProcessStartInfo(fileLocation)
            {
                Arguments = Path.GetFileName(fileLocation),
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(fileLocation),
                //FileName = "D:\\Apps\\Sublime\\Sublime Text 3\\sublime_text.exe";//Dylan Path
                FileName = "D:\\Program Files\\Sublime Text 3\\sublime_text.exe",//Tyler path
                Verb = "OPEN"
            };
            Process.Start(pi);
        }
    }
}
