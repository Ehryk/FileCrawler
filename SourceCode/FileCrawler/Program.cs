using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common;
using Common.Extensions;
using SevenZip;
using Common.Logging;

namespace FileCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Enable Longer History in Window
            Console.BufferHeight = 8000;
            Console.Title = String.Format("{0} v{1}", AssemblyInfo.ProductName, AssemblyInfo.Version);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" --- {0} v{1}, {2} ---", AssemblyInfo.ProductName, AssemblyInfo.Version, AssemblyInfo.CompanyName);
            Console.ForegroundColor = ConsoleColor.White;

            if (AppSettings.Compressed_ReadContents)
            {
                string sevenZipLocation;
                if (AppSettings.Compressed_PathTo7zDLL == null)
                {
                    sevenZipLocation = Path.Combine(Path.GetDirectoryName(AssemblyInfo.Path), "7z.dll");
                    Create7zDLL(sevenZipLocation);
                }
                else
                    sevenZipLocation = AppSettings.Compressed_PathTo7zDLL;

                ConfigurationManager.AppSettings["7zLocation"] = sevenZipLocation; 
            }

            CrawlType type = CrawlType.Full;
            if (args.Length > 1 && !String.IsNullOrWhiteSpace(args[1]))
            {
                type = (CrawlType)Enum.Parse(typeof(CrawlType), args[1]);
            }

            FileCrawler crawler = new FileCrawler(args[0], type);
            crawler.StartCrawl();

            foreach (FileData data in crawler.Files)
            {
                Console.WriteLine(String.Format(@"{0} {1} {2:N1} KB", data.Path, data.Extension, data.KB));
            }

            if (AppSettings.ReportDirectories)
            {
                Console.WriteLine();
                Console.WriteLine("Directories:");
                foreach (DirectoryData data in crawler.Directories)
                {
                    Console.WriteLine(String.Format(@"{0} {1} {2:N1} MB", data.Path, data.FileCount, data.TotalSize.GetMB()));
                }
            }

            if (crawler.InaccessibleFiles.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Inaccessible Files:");
                foreach (string path in crawler.InaccessibleFiles)
                {
                    Console.WriteLine(String.Format(@"{0}", path));
                }
            }

            if (crawler.InaccessibleDirectories.Count > 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Inaccessible Directories:");
                foreach (string path in crawler.InaccessibleDirectories)
                {
                    Console.WriteLine(String.Format(@"{0}", path));
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Crawl Complete: discovered {0} files in {1}h {2}m {3}.{4:N2}s ({5:N2} MB).", crawler.FileCount, crawler.CrawlTime.Value.Hours, crawler.CrawlTime.Value.Minutes, crawler.CrawlTime.Value.Seconds, crawler.CrawlTime.Value.Milliseconds, crawler.TotalSizeMB);

            Console.ResetColor();

            if (AppSettings.LeaveConsoleOpen)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static bool Create7zDLLs()
        {
            try
            {
                ExtractEmbeddedResource("7z-x86.dll", "7z.dll");
                ExtractEmbeddedResource("7z-x64.dll", "7z64.dll");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not extract embedded 7z DLLs");
                return false;
            }
        }

        private static bool Create7zDLL(string destination)
        {
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    ExtractEmbeddedResource("7z-x64.dll", destination, true);
                }
                else
                    ExtractEmbeddedResource("7z-x86.dll", destination, true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not extract embedded {0}-bit 7z DLL", Environment.Is64BitOperatingSystem ? 64 : 32);
                return false;
            }
        }

        private static string ExtractEmbeddedResource(string resourceName, string destination = null, bool overwrite = false)
        {
            FileInfo info = new FileInfo(destination);
            if (info.Exists)
            {
                if (overwrite)
                    File.Delete(destination);
                else
                    return info.FullName;
            }

            string resourceLocator = String.Format("{0}.{1}.{2}", AssemblyInfo.Name, "Resources", resourceName);
            using (Stream szdllStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceLocator))
            {
                if (szdllStream == null)
                    throw new Exception(String.Format("Embedded Resource not found: {0}", resourceLocator));

                using (FileStream fs = File.Create(destination))
                {
                    int bufSize = 1024;
                    byte[] buf = new byte[bufSize];
                    int cnt;
                    while ((cnt = szdllStream.Read(buf, 0, bufSize)) > 0)
                    {
                        fs.Write(buf, 0, cnt);
                    }
                }
            }

            return info.FullName;
        }
    }
}
