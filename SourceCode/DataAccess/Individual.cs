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

namespace DataAccess
{
    public class Individual : IDataAccess
    {
        public string Name() { return "Individual"; }

        public bool UsesCrawlStart() { return true; }
        public bool UsesDirectoryFound() { return false; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return true; }
        public bool UsesDirectoryProcessed() { return false; }
        public bool UsesCrawlError() { return true; }
        public bool UsesFileInaccessible() { return false; }
        public bool UsesDirectoryInaccessible() { return false; }
        public bool UsesCrawlEnd() { return true; }

        public void CrawlStart(object sender, EventArgs e)
        {
            Logger.LogDebug("DataAccess Test: Crawl Started.");
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            //Insert File
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
            Logger.LogError(e.GetException(), "DataAccess Test: Error");
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            Logger.LogDebug("DataAccess Test: Crawl Ended.");
        }
    }
}
