using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.dropbox.com/s/1pfe7xlsbgjbaan/GLEE%20-%20Last%20Christmas%20%28Full%20Performance%29%20HD-ver3.rar?dl=0";
            url = getDirectLink(url);
            DownloadFile(url);
            Console.ReadLine();
        }

        private static string getDirectLink(string url)
        {
            if (url.Contains("www.dropbox.com"))
            {
                url = url.Replace("www.dropbox.com", "dl.dropboxusercontent.com");
            }

            return url;
        }

        private static void DownloadFile(string url)
        {
            DateTime start = DateTime.Now;
            Uri uri = new Uri(url);
            string filename = uri.AbsolutePath.Substring(uri.AbsolutePath.LastIndexOf("/") + 1);
            filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            long position = 0;
            FileInfo info = new FileInfo(filename);
            if (info.Exists)
            {
                position = info.Length;
                fs.Seek(position, SeekOrigin.Current);
            }
            request.AddRange(position);
            Stream stream = request.GetResponse().GetResponseStream();
            byte[] bytes = new byte[1024 * 512];
            int readCount = 0;
            int totalCount = 0;
            while (true)
            {
                readCount = stream.Read(bytes, 0, bytes.Length);
                if (readCount <= 0)
                    break;
                fs.Write(bytes, 0, readCount);
                fs.Flush();
                totalCount = totalCount + readCount;
                Console.WriteLine("已下載大小：" + totalCount);
            }
            fs.Close();
            stream.Close();
            Console.WriteLine("文件下載成功，耗時：" + (DateTime.Now - start).TotalSeconds + "秒");
        }
    }
}
