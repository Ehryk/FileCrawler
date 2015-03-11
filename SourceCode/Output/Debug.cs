using System;
using Common.Events;
using Common.Extensions;
using Common.Interfaces;
using System.IO;

namespace Output
{
    public class Debug : IOutput
    {
        #region Properties

        public bool ReportStartEnd = true;
        public bool ReportDirectories = true;

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

        public bool UsesCrawlStart() { return ReportStartEnd; }
        public bool UsesDirectoryFound() { return ReportDirectories; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return true; }
        public bool UsesDirectoryProcessed() { return ReportDirectories; }
        public bool UsesCrawlError() { return true; }
        public bool UsesFileInaccessible() { return true; }
        public bool UsesDirectoryInaccessible() { return true; }
        public bool UsesCrawlEnd() { return ReportStartEnd; }

        public void CrawlStart(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("Crawl Started");
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
            System.Diagnostics.Debug.Print("Processing Directory {0}...", e.DirectoryData.Path);
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            System.Diagnostics.Debug.Print("{0} ({1:N2} MB)", e.FileData.Path, e.FileData.MB);
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
            System.Diagnostics.Debug.Print("Processed Directory {0} ({1} files, {2:N2} MB)", e.DirectoryData.Path, e.DirectoryData.FileCount, e.DirectoryData.TotalSize.GetMB());
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
            System.Diagnostics.Debug.Print("Error: {0}", e.GetException().Message);
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
            System.Diagnostics.Debug.Print("{0} Inaccessible", e.Path);
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
            System.Diagnostics.Debug.Print("{0} Inaccessible", e.Path);
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Print("Crawl Ended");
        }

        #endregion

        #region Methods

        #endregion
    }
}
