using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace spa.web
{
    public class HtmlSnapshot
    {
        public static string GetSnapshot(string url)
        {
            var appRoot = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var startInfo = new ProcessStartInfo
            {
                Arguments = String.Format("{0} {1}", Path.Combine(appRoot, "Scripts\\libs\\createSnapshot.js"), url),
                FileName = Path.Combine(appRoot, "bin\\phantomjs.exe"),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };
            var p = new Process();
            p.StartInfo = startInfo;
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public static bool IsCrawler(string userAgent)
        {
            var keyWords = new[] { "bot", "crawl", "slurp", "spider" };
            return keyWords.Any(userAgent.Contains);
        }
    }
}