using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Extensions;

namespace FileCrawler
{
    class FileCrawler
    {
        #region Private Properties

        private bool crawlComplete = false;
        private Stopwatch stopwatch = new Stopwatch();

        private string path;
        private CrawlType type;
        private List<FileData> files;
        private List<string> inaccessibleFiles;
        private List<DirectoryData> directories;
        private List<string> inaccessibleDirectories;

        private int fileCount;
        private int directoryCount;
        private long totalSize;
        private double totalSizeKB;
        private double totalSizeMB;
        private double totalSizeGB;

        #endregion

        #region Public Properties

        public bool CrawlComplete
        {
            get { return crawlComplete; }
        }

        public TimeSpan? CrawlTime
        {
            get 
            {
                if (crawlComplete)
                    return stopwatch.Elapsed;
                else
                    return null;
            }
        }

        public CrawlType Type
        {
            get { return type; }
        }

        public List<FileData> Files
        {
            get { return files; }
        }

        public List<DirectoryData> Directories
        {
            get { return directories; }
        }

        public int FileCount
        {
            get { return fileCount; }
        }

        public int DirectoryCount
        {
            get { return directoryCount; }
        }

        public long TotalSize
        {
            get { return totalSize; }
        }

        public double TotalSizeKB
        {
            get { return totalSizeKB; }
        }

        public double TotalSizeMB
        {
            get { return totalSizeMB; }
        }

        public double TotalSizeGB
        {
            get { return totalSizeGB; }
        }

        #endregion

        #region Constructors

        public FileCrawler(string pPath, CrawlType pType = CrawlType.Full)
        {
            path = pPath;
            type = pType;

            files = new List<FileData>();
            directories = new List<DirectoryData>();

            inaccessibleFiles = new List<string>();
            inaccessibleDirectories = new List<string>();
        }

        #endregion

        #region Public Methods

        public bool StartCrawl()
        {
            bool success = false;
            stopwatch.Start();
            int retCode = 0;

            try
            {
                ProcessDirectory(path, ref retCode, type);
                success = true;
            }
            catch (Exception ex)
            {
                //Log
            }

            stopwatch.Stop();
            crawlComplete = true;
            return success;
        }

        #endregion

        #region Private Methods

        private int ProcessDirectory(string pPath, ref int pRetCode, CrawlType? pType = null, bool pLoadSubdirectories = true, bool pIsSubdirectory = false)
        {
            CrawlType type = pType ?? Type;

            DirectoryInfo directory = new DirectoryInfo(pPath);
            if (!directory.Exists)
                throw new DirectoryNotFoundException(String.Format("{0} {1} not found.", pIsSubdirectory ? "Subdirectory" : "Directory", directory.FullName));

            List<FileInfo> filesToProcess = Utilities.GetFilesToProcess(directory, type, pIsSubdirectory);
            
            int processed = 0;

            foreach (FileInfo info in filesToProcess)
            {
                try
                {
                    FileData data = new FileData(info);

                    ProcessFile(data);

                    processed++;
                }
                catch (Exception ex)
                {
                    inaccessibleFiles.Add(info.FullName);
                }
            }

            //Subdirectory Processing is Optional for these crawl types; specify with pLoadSubdirectories
            if (pLoadSubdirectories && (type == CrawlType.Full || type == CrawlType.Shallow))
            {
                List<DirectoryInfo> directoriesToProcess = Utilities.GetDirectoriesToProcess(directory, type);
                foreach (DirectoryInfo info in directoriesToProcess)
                {
                    try
                    {
                        DirectoryData data = new DirectoryData(info);
                        directories.Add(data);
                        directoryCount++;
                    }
                    catch (Exception ex)
                    {
                        inaccessibleDirectories.Add(info.FullName);
                    }
                }
            }

            //Subdirectory Processing is mandatory for these crawl types
            if (type == CrawlType.RecurseSubdirectories || type == CrawlType.Recursive)
            {
                List<DirectoryInfo> directoriesToProcess = Utilities.GetDirectoriesToProcess(directory, type);
                foreach (DirectoryInfo info in directoriesToProcess)
                {
                    try
                    {
                        DirectoryData data = new DirectoryData(info);
                        directories.Add(data);
                        processed += ProcessDirectory(info.FullName, ref pRetCode, type, pLoadSubdirectories, true);
                    }
                    catch (Exception ex)
                    {
                        inaccessibleDirectories.Add(info.FullName);
                    }
                }
            }

            return processed;
        }

        private bool ProcessFile(FileData data)
        {
            data.IsCompressedContainer = Utilities.IsContainer(data);

            if (data.IsCompressedContainer && !data.IsContained)
            {
                //Container file, not contained within another container
                if (AppSettings.Compressed_ReportContainers)
                {
                    //Add Container
                    files.Add(data);
                    fileCount++;
                    totalSize += data.Size;
                    UpdateSizes();
                }

                if (AppSettings.Compressed_ReadContents)
                {
                    List<FileData> contents = Utilities.ReadContainerContents(data.Path, data);

                    foreach (FileData cData in contents)
                    {
                        ProcessFile(cData);
                    }
                }

            }
            else if (data.IsCompressedContainer && AppSettings.Compressed_ReadContents_Recurse)
            {
                //Container file, nested within another container
                if (AppSettings.Compressed_ReportContainers)
                {
                    //Add Container
                    files.Add(data);
                    fileCount++;
                    totalSize += data.Size;
                    UpdateSizes();
                }

                string localCopy = Utilities.ExtractFile(data);

                List<FileData> contents = Utilities.ReadContainerContents(localCopy, data);

                foreach (FileData cData in contents)
                {
                    ProcessFile(cData);
                }
            }
            else
            {
                //Non Container File or a nested container when not set to recurse
                files.Add(data);
                fileCount++;
                totalSize += data.Size;
                UpdateSizes();
            }

            return true;
        }

        private bool UpdateSizes()
        {
            totalSizeKB = totalSize.GetKB();
            totalSizeMB = totalSize.GetMB();
            totalSizeGB = totalSize.GetGB();

            return true;
        }

        #endregion
    }
}
