﻿using System;
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
    public class Null : IOutput
    {
        public string Name() { return "Null"; }

        public bool UsesCrawlStart() { return false; }
        public bool UsesDirectoryFound() { return false; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return false; }
        public bool UsesDirectoryProcessed() { return false; }
        public bool UsesCrawlError() { return false; }
        public bool UsesFileInaccessible() { return false; }
        public bool UsesDirectoryInaccessible() { return false; }
        public bool UsesCrawlEnd() { return false; }

        public void CrawlStart(object sender, EventArgs e)
        {
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
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
        }
    }
}
