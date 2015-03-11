using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Events;
using Common.Interfaces;
using Common.Logging;
using System.IO;

namespace Output
{
    public class Logger : IOutput
    {
        #region Properties

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

        public bool UsesCrawlStart() { return true; }
        public bool UsesDirectoryFound() { return true; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return true; }
        public bool UsesDirectoryProcessed() { return true; }
        public bool UsesCrawlError() { return false; } //Already Logged
        public bool UsesFileInaccessible() { return true; }
        public bool UsesDirectoryInaccessible() { return true; }
        public bool UsesCrawlEnd() { return true; }

        public void CrawlStart(object sender, EventArgs e)
        {
            Common.Logging.Logger.LogDebug("Crawl Started.");
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
            Common.Logging.Logger.LogDebug("Entering Directory {0}.", e.DirectoryData.Path);
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            Common.Logging.Logger.LogInfo("File Processed: {0} ({1:N2} MB).", e.FileData.Path, e.FileData.MB);
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
            Common.Logging.Logger.LogDebug("Processed Directory {0}.", e.DirectoryData.Path);
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
            //Already Logged
            Common.Logging.Logger.LogError(e.GetException(), "Crawl Error");
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
            Common.Logging.Logger.LogWarning("File Inaccessible: {0}", e.Path);
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
            Common.Logging.Logger.LogWarning("Directory Inaccessible: {0}", e.Path);
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            Common.Logging.Logger.LogDebug("Crawl Ended.");
        }

        #endregion

        #region Methods

        #endregion
    }
}
