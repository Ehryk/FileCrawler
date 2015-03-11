using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Common.Events;
using Common.Enums;
using Common.Extensions;
using Common.Logging;
using Common.Interfaces;
using Common.Objects;

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
        private List<DirectoryData> directories;
        private List<string> inaccessibleFiles;
        private List<string> inaccessibleDirectories;

        private int fileCount;
        private int directoryCount;
        private long totalSize;

        #endregion

        #region Public Properties

        private DirectoryData rootDirectory;

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

        public List<string> InaccessibleFiles
        {
            get { return inaccessibleFiles; }
        }

        public List<string> InaccessibleDirectories
        {
            get { return inaccessibleDirectories; }
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

        public decimal TotalSizeKB
        {
            get { return totalSize.GetKB(); }
        }

        public decimal TotalSizeMB
        {
            get { return totalSize.GetMB(); }
        }

        public decimal TotalSizeGB
        {
            get { return totalSize.GetGB(); }
        }

        #endregion

        #region Events

        public event EventHandler OnCrawlStart;
        public event DirectoryDataEventHandler OnDirectoryFound;
        public event FileDataEventHandler OnFileFound;
        public event FileDataEventHandler OnFileProcessed;
        public event DirectoryDataEventHandler OnDirectoryFilesProcessed;
        public event DirectoryDataEventHandler OnDirectoryProcessed;
        public event ErrorEventHandler OnCrawlError;
        public event InaccessibleEventHandler OnFileInaccessible;
        public event InaccessibleEventHandler OnDirectoryInaccessible;
        public event EventHandler OnCrawlEnd;

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
                if (OnCrawlStart != null)
                    OnCrawlStart(this, EventArgs.Empty);

                if (path.IsDirectory())
                {
                    DirectoryInfo rootInfo = new DirectoryInfo(path);
                    rootDirectory = new DirectoryData(rootInfo);
                    ProcessDirectory(rootDirectory, ref retCode, type);
                }
                else
                {
                    FileInfo rootInfo = new FileInfo(path);
                    FileData file = new FileData(rootInfo);
                    ProcessFile(file);
                }
                success = retCode == 0;
            }
            catch (Exception ex)
            {
                if (OnCrawlError != null)
                    OnCrawlError(this, new ErrorEventArgs(ex));

                Logger.LogError(ex, "Could not process {0}", path);
            }
            finally
            {
                if (OnCrawlEnd != null)
                    OnCrawlEnd(this, EventArgs.Empty);
            }

            stopwatch.Stop();
            crawlComplete = true;
            return success;
        }

        public bool AttachOutput(IOutput pOutput)
        {
            try
            {
                if (pOutput.UsesCrawlStart())
                    this.OnCrawlStart += pOutput.CrawlStart;
                if (pOutput.UsesDirectoryFound())
                    this.OnDirectoryFound += pOutput.DirectoryFound;
                if (pOutput.UsesFileFound())
                    this.OnFileFound += pOutput.FileFound;
                if (pOutput.UsesFileProcessed())
                    this.OnFileProcessed += pOutput.FileProcessed;
                if (pOutput.UsesDirectoryProcessed())
                    this.OnDirectoryProcessed += pOutput.DirectoryProcessed;
                if (pOutput.UsesCrawlError())
                    this.OnCrawlError += pOutput.CrawlError;
                if (pOutput.UsesFileInaccessible())
                    this.OnFileInaccessible += pOutput.FileInaccessible;
                if (pOutput.UsesDirectoryInaccessible())
                    this.OnDirectoryInaccessible += pOutput.DirectoryInaccessible;
                if (pOutput.UsesCrawlEnd())
                    this.OnCrawlEnd += pOutput.CrawlEnd;

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "DataAccess {0} Attachment failed", pOutput.Name());
                return false;
            }
        }

        #endregion

        #region Private Methods

        private int ProcessDirectory(DirectoryData pDirectory, ref int pRetCode, CrawlType? pType = null, bool pLoadSubdirectories = true, bool pIsSubdirectory = false)
        {
            CrawlType type = pType ?? Type;

            DirectoryInfo directory = new DirectoryInfo(pDirectory.Path);
            if (!directory.Exists)
                throw new DirectoryNotFoundException(String.Format("{0} {1} not found.", pIsSubdirectory ? "Subdirectory" : "Directory", directory.FullName));

            if (OnDirectoryFound != null)
                OnDirectoryFound(this, new DirectoryDataEventArgs(pDirectory));

            List<FileInfo> filesToProcess = CrawlUtilities.GetFilesToProcess(directory, type, pIsSubdirectory);
            
            int processed = 0;

            foreach (FileInfo info in filesToProcess)
            {
                try
                {
                    FileData data = new FileData(info);
                    ProcessFile(data, ref pDirectory);
                    processed++;
                }
                catch (PathTooLongException ex)
                {
                    string fileName = CrawlUtilities.GetFullPath(info);
                    Logger.LogWarning(ex, "Path too Long: {0}", fileName);
                    inaccessibleFiles.Add(fileName);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "File {0} is inaccessible", info.FullName);

                    if (OnFileInaccessible != null)
                        OnFileInaccessible(this, new InaccessibleEventArgs(info, ex));

                    inaccessibleFiles.Add(info.FullName);
                }
            }

            if (OnDirectoryFilesProcessed != null)
                OnDirectoryFilesProcessed(this, new DirectoryDataEventArgs(pDirectory, true));

            //Subdirectory Processing is Optional for these crawl types; specify with pLoadSubdirectories
            if ((type == CrawlType.Full || type == CrawlType.Shallow) && (pLoadSubdirectories || AppSettings.ReportDirectories))
            {
                List<DirectoryInfo> directoriesToProcess = CrawlUtilities.GetDirectoriesToProcess(directory, type);
                foreach (DirectoryInfo info in directoriesToProcess)
                {
                    try
                    {
                        DirectoryData data = new DirectoryData(info);

                        if (OnDirectoryFound != null)
                            OnDirectoryFound(this, new DirectoryDataEventArgs(data));

                        directories.Add(data);
                        directoryCount++;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Directory {0} is inaccessible", info.FullName);

                        if (OnDirectoryInaccessible != null)
                            OnDirectoryInaccessible(this, new InaccessibleEventArgs(info, ex));

                        inaccessibleDirectories.Add(info.FullName);
                    }
                }
            }

            //Subdirectory Processing is mandatory for these crawl types
            if (type == CrawlType.RecurseSubdirectories || type == CrawlType.Recursive)
            {
                List<DirectoryInfo> directoriesToProcess = CrawlUtilities.GetDirectoriesToProcess(directory, type);
                foreach (DirectoryInfo info in directoriesToProcess)
                {
                    try
                    {
                        DirectoryData data = new DirectoryData(info);

                        processed += ProcessDirectory(data, ref pRetCode, type, pLoadSubdirectories, true);
                        directories.Add(data);
                        directoryCount++;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Directory {0} is inaccessible", info.FullName);

                        if (OnDirectoryInaccessible != null)
                            OnDirectoryInaccessible(this, new InaccessibleEventArgs(info, ex));

                        inaccessibleDirectories.Add(info.FullName);
                    }
                }
            }

            if (OnDirectoryProcessed != null)
                OnDirectoryProcessed(this, new DirectoryDataEventArgs(pDirectory, true, true));

            return processed;
        }

        private bool ProcessFile(FileData data)
        {
            DirectoryData fake = new DirectoryData("file://");
            return ProcessFile(data, ref fake);
        }

        private bool ProcessFile(FileData data, ref DirectoryData directory)
        {
            data.IsCompressedContainer = CrawlUtilities.IsContainer(data);

            if (OnFileFound != null)
                OnFileFound(this, new FileDataEventArgs(data, directory));

            if (data.IsCompressedContainer && !data.IsContained)
            {
                //Container file, not contained within another container
                if (AppSettings.Compressed_ReportContainers)
                {
                    //Add Container
                    AddFile(data, directory);
                }

                if (AppSettings.Compressed_ReadContents)
                {
                    List<FileData> contents = CrawlUtilities.ReadContainerContents(data.Path, data);

                    foreach (FileData cData in contents)
                    {
                        ProcessFile(cData, ref directory);
                    }
                }
            }
            else if (data.IsCompressedContainer && AppSettings.Compressed_ReadContents_Recurse)
            {
                //Container file, nested within another container
                if (AppSettings.Compressed_ReportContainers)
                {
                    //Add Container
                    AddFile(data, directory);
                }

                string localCopy = CrawlUtilities.ExtractFile(data);

                List<FileData> contents = CrawlUtilities.ReadContainerContents(localCopy, data);

                foreach (FileData cData in contents)
                {
                    ProcessFile(cData, ref directory);
                }
            }
            else
            {
                //Non Container File or a nested container when not set to recurse
                AddFile(data, directory);
            }

            directory.FileCount++;
            directory.TotalSize += data.Size;
            return true;
        }

        private bool AddFile(FileData pFile, DirectoryData pParent)
        {
            files.Add(pFile);
            fileCount++;
            totalSize += pFile.Size;

            if (OnFileProcessed != null)
                OnFileProcessed(this, new FileDataEventArgs(pFile, pParent, true));

            return true;
        }

        #endregion
    }
}
