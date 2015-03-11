﻿using System;
using System.IO;
using System.Linq;
using System.Configuration;
using Common.Enums;
using Common.Logging;
using Common.Extensions;
using Common.Objects;

namespace FileCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Enable Longer History in Window
            Console.BufferHeight = 8000;
            Console.Title = String.Format("{0} v{1}", AssemblyInfo.ProductName, AssemblyInfo.Version);

            if (!AppSettings.Quiet)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" --- {0} v{1}, {2} ---", AssemblyInfo.ProductName, AssemblyInfo.Version, AssemblyInfo.CompanyName);
                Console.ForegroundColor = ConsoleColor.White;
            }

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

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Crawling {0}... ", args[0]);

            FileCrawler crawler = new FileCrawler(args[0], type);
            //crawler.AttachDataAccess(new DataAccess.Test());
            crawler.StartCrawl();

            Console.WriteLine("Done.");
            Console.WriteLine();

            if (!AppSettings.Quiet)
            {
                foreach (FileData data in crawler.Files)
                {
                    Console.WriteLine(@"{0} {1} {2:N1} KB", data.Path, data.Extension, data.KB);
                }
                Console.WriteLine();

                if (AppSettings.ReportDirectories)
                {
                    Console.WriteLine("Directories:");
                    foreach (DirectoryData data in crawler.Directories)
                    {
                        Console.WriteLine(@"{0} {1} {2:N1} MB", data.Path, data.FileCount, data.TotalSize.GetMB());
                    }
                    Console.WriteLine();
                }
            }

            if (crawler.InaccessibleFiles.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Inaccessible Files:");
                foreach (string path in crawler.InaccessibleFiles)
                {
                    Console.WriteLine(@"{0}", path);
                }
                Console.WriteLine();
            }

            if (crawler.InaccessibleDirectories.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Inaccessible Directories:");
                foreach (string path in crawler.InaccessibleDirectories)
                {
                    Console.WriteLine(@"{0}", path);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Crawl Complete: discovered {0} files in {1} directories.", crawler.FileCount, crawler.DirectoryCount + 1);
            if (crawler.CrawlTime != null)
                Console.WriteLine("Crawl Time: {0}h {1}m {2}.{3}s ({4:N2} MB).", crawler.CrawlTime.Value.Hours, crawler.CrawlTime.Value.Minutes, crawler.CrawlTime.Value.Seconds, crawler.CrawlTime.Value.Milliseconds, crawler.TotalSizeMB);

            Console.WriteLine("{0} containers found with {1} files contained ({2:N2} MB)", crawler.Files.Count(f => f.IsCompressedContainer), crawler.Files.Count(f => f.IsContained), crawler.Files.Where(f => f.IsCompressedContainer).Sum(f => f.MB));
            Console.WriteLine();

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
