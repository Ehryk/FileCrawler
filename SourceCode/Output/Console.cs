﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Events;
using Common.Extensions;
using Common.Interfaces;
using Common.Logging;
using System.IO;

namespace Output
{
    public class Console : IOutput
    {
        #region Properties

        public ConsoleColor SummaryColor = ConsoleColor.Cyan;
        public ConsoleColor DirectoryColor = ConsoleColor.Cyan;
        public ConsoleColor FileColor = ConsoleColor.Gray;
        public ConsoleColor ErrorColor = ConsoleColor.Red;

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

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
            WriteLine(SummaryColor, "Crawl Started {0}.", DateTime.Now.ToShortTimeString());
            WriteLine();
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
            WriteLine(DirectoryColor, "Processing Directory {0}...", e.DirectoryData.Path);
        }

        public void FileFound(object sender, FileDataEventArgs e)
        {
        }

        public void FileProcessed(object sender, FileDataEventArgs e)
        {
            WriteLine(FileColor, "{0} ({1:N2} MB)", e.FileData.Path);
        }

        public void DirectoryProcessed(object sender, DirectoryDataEventArgs e)
        {
            WriteLine(DirectoryColor, "Processed Directory {0} ({1} files, {2:N2} MB)", e.DirectoryData.Path, e.DirectoryData.FileCount, e.DirectoryData.TotalSize.GetMB());
        }

        public void CrawlError(object sender, ErrorEventArgs e)
        {
            WriteLine(ErrorColor, "Error: {0}", e.GetException().Message);
        }

        public void FileInaccessible(object sender, InaccessibleEventArgs e)
        {
            WriteLine(ErrorColor, "{0} Inaccessible", e.Path);
        }

        public void DirectoryInaccessible(object sender, InaccessibleEventArgs e)
        {
            WriteLine(ErrorColor, "{0} Inaccessible", e.Path);
        }

        public void CrawlEnd(object sender, EventArgs e)
        {
            WriteLine(SummaryColor, "Crawl Ended {0}.", DateTime.Now.ToShortTimeString());
            WriteLine();
            System.Console.ResetColor();
        }

        #endregion

        #region Methods

        private void WriteLine(string value = "", params object[] args)
        {
            System.Console.WriteLine(value, args);
        }

        private void WriteLine(ConsoleColor color, string value = "", params object[] args)
        {
            System.Console.ForegroundColor = color;
            System.Console.WriteLine(value, args);
        }

        private void Write(string value = "", params object[] args)
        {
            System.Console.Write(value, args);
        }

        private void Write(ConsoleColor color, string value = "", params object[] args)
        {
            System.Console.ForegroundColor = color;
            System.Console.Write(value, args);
        }

        #endregion
    }
}