using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace spa.web
{
    public class FileHelper
    {
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public static bool WaitFileUnlocked(string path, int interations, int time) 
        {
            for (var i = 0; i < interations; i++)
            {
                if (!IsFileLocked(new FileInfo(path)))
                    return true;
                Thread.Sleep(time);
            }
            return false;
        }

        public static void WaitFileUnlockedAsync(Action action, string path, int interations, int time)
        {
            new Thread(() => {
                if (WaitFileUnlocked(path, interations, time)) {
                    action();
                }
            }).Start();
        }

        public static void DeleteFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
                WaitFileUnlockedAsync(() => { File.Delete(file); Debug.WriteLine("XXXXXXXXXXXXXXXXXXX-XXXXXX  " + file); }, file, 10, 800);
        }
    }
}