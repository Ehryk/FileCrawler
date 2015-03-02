using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Extensions;

namespace FileCrawler
{
    static class Utilities
    {
        public static List<string> EnumerateFileSystemEntries(string path, CrawlType type = CrawlType.Full)
        {
            List<string> results;

            switch (type)
            {
                case CrawlType.Full:
                default:
                    results = Directory.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories).ToList();
                    break;

                case CrawlType.Shallow:
                case CrawlType.Recursive:
                    results = Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).ToList();
                    break;

                case CrawlType.RecurseSubdirectories:
                    results = Directory.EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly).ToList();
                    break;
            }

            return results;
        }

        public static List<DirectoryInfo> GetDirectoriesToProcess(DirectoryInfo info, CrawlType type = CrawlType.Full, bool pIsSubdirectory = false)
        {
            if (!info.Exists)
                throw new DirectoryNotFoundException(String.Format("{0} {1} not found.", pIsSubdirectory ? "Subdirectory" : "Directory", info.FullName));

            List<DirectoryInfo> results;

            switch (type)
            {
                case CrawlType.Full:
                default:
                    results = info.GetDirectories("*", SearchOption.AllDirectories).ToList();
                    break;

                case CrawlType.Shallow:
                case CrawlType.Recursive:
                    results = info.GetDirectories("*", SearchOption.TopDirectoryOnly).ToList();
                    break;

                case CrawlType.RecurseSubdirectories:
                    if (pIsSubdirectory)
                        results = info.GetDirectories("*", SearchOption.AllDirectories).ToList();
                    else
                        results = info.GetDirectories("*", SearchOption.TopDirectoryOnly).ToList();
                    break;
            }

            return results;
        }

        public static List<FileInfo> GetFilesToProcess(DirectoryInfo info, CrawlType type = CrawlType.Full, bool pIsSubdirectory = false)
        {
            if (!info.Exists)
                throw new DirectoryNotFoundException(String.Format("{0} {1} not found.", pIsSubdirectory ? "Subdirectory" : "Directory", info.FullName));

            List<FileInfo> results;

            switch (type)
            {
                case CrawlType.Full:
                default:
                    results = info.GetFiles("*", SearchOption.AllDirectories).ToList();
                    break;

                case CrawlType.Shallow:
                case CrawlType.Recursive:
                    results = info.GetFiles("*", SearchOption.TopDirectoryOnly).ToList();
                    break;

                case CrawlType.RecurseSubdirectories:
                    if (pIsSubdirectory)
                        results = info.GetFiles("*", SearchOption.AllDirectories).ToList();
                    else
                        results = info.GetFiles("*", SearchOption.TopDirectoryOnly).ToList();
                    break;
            }

            return results;
        }

        public static bool SeparateFileSystemEntries(IEnumerable<string> entries, ref List<string> files, ref List<string> subdirectories)
        {
            int processed = 0;

            foreach (string entry in entries)
            {
                if (entry.IsDirectory())
                    subdirectories.Add(entry);
                else
                    files.Add(entry);

                processed++;
            }

            return processed == entries.Count();
        }
    }
}
