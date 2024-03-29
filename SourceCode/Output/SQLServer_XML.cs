﻿using System;
using Common.Events;
using Common.Interfaces;
using System.IO;

namespace Output
{
    public class SQLServer_XML : IOutput
    {
        #region Properties

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

        public bool UsesCrawlStart() { return true; }
        public bool UsesDirectoryFound() { return false; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return true; }
        public bool UsesDirectoryProcessed() { return true; }
        public bool UsesCrawlError() { return false; }
        public bool UsesFileInaccessible() { return false; }
        public bool UsesDirectoryInaccessible() { return false; }
        public bool UsesCrawlEnd() { return true; }

        public void CrawlStart(object sender, EventArgs e)
        {
            //Clear Loading
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            if (e.FileData.IsCompressedContainer)
            {
                //Insert Container's Files
            }
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
            //Insert Directory's Files
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            //Perform Diff
        }

        #endregion

        #region Methods

        #endregion
    }
}
