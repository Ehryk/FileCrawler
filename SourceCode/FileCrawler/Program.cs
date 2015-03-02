using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Extensions;

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

            FileCrawler crawler = new FileCrawler(@"C:\Projects\FileCrawler", CrawlType.Full);
            crawler.StartCrawl();

            foreach(FileData data in crawler.Files)
            {
                Console.WriteLine(String.Format(@"{0}\{1} {2:N1} KB", data.Directory, data.Name, data.KB));
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
    }
}
