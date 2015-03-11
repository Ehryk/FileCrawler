using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Enums;
using Common.Extensions;
using SevenZip;

namespace FileCrawler
{
    public static class CrawlUtilities
    {
        #region File Utilities

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

        public static bool IsContainer(FileData data)
        {
            string extension = data.Extension.Replace(".", "");
            return AppSettings.Compressed_Extensions.Any(ext => ext.EqualsIgnoreCase(extension));
        }

        public static bool IsDirectory(this string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static string GetFullPath(FileInfo src)
        {
            return (string)src.GetType()
                .GetField("FullPath", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(src);
        }

        #endregion

        #region Compressed Containers

        public static List<FileData> ReadContainerContents(string path, FileData container)
        {
            List<FileData> contents = new List<FileData>();

            SevenZipExtractor extractor = new SevenZipExtractor(path);
            foreach (ArchiveFileInfo info in extractor.ArchiveFileData.Where(fd => !fd.IsDirectory))
            {
                FileData data = new FileData(info, container);
                
                if (AppSettings.Compressed_Extensions.Any(ext => ext.EqualsIgnoreCase(data.Extension)))
                {
                    data.IsCompressedContainer = true;
                }

                contents.Add(data);
            }

            return contents;
        }

        public static string ExtractFile(FileData data, string destination = null)
        {
            destination = destination ?? AppSettings.ProcessingTemp;
            string target = Path.Combine(destination, data.Name);

            SevenZipExtractor extractor = new SevenZipExtractor(data.ContainerPath);
            FileStream fs = File.OpenWrite(target);
            extractor.ExtractFile(data.ContainedPath, fs);

            return target;
        }

        #endregion
    }
}
