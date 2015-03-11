using System;
using Common.Events;
using Common.Extensions;
using Common.Interfaces;
using System.IO;

namespace Output
{
    public class Console : IOutput
    {
        #region Properties

        public bool ReportStartEnd = true;
        public bool ReportDirectories = true;
        public bool ReportInaccessible = true;
        public bool ReportFiles = true;

        public ConsoleColor SummaryColor = ConsoleColor.Cyan;
        public ConsoleColor DirectoryColor = ConsoleColor.Cyan;
        public ConsoleColor FileColor = ConsoleColor.Gray;
        public ConsoleColor ErrorColor = ConsoleColor.Red;

        #endregion

        #region IOutput Members

        public string Name() { return GetType().Name; }

        public bool UsesCrawlStart() { return ReportStartEnd; }
        public bool UsesDirectoryFound() { return ReportDirectories; }
        public bool UsesFileFound() { return false; }
        public bool UsesFileProcessed() { return ReportFiles; }
        public bool UsesDirectoryProcessed() { return ReportDirectories; }
        public bool UsesCrawlError() { return true; }
        public bool UsesFileInaccessible() { return ReportInaccessible; }
        public bool UsesDirectoryInaccessible() { return ReportInaccessible; }
        public bool UsesCrawlEnd() { return ReportStartEnd; }

        public void CrawlStart(object sender, EventArgs e)
        {
            WriteLine(SummaryColor, "Crawl Started {0}.", DateTime.Now.ToShortTimeString());
            WriteLine();
        }

        public void DirectoryFound(object sender, DirectoryDataEventArgs e)
        {
            if (ReportFiles)
                WriteLine(DirectoryColor, "Processing Directory {0}...", e.DirectoryData.Path);
            else
                Write(DirectoryColor, "Processing Directory {0}... ", e.DirectoryData.Path);
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
            if (ReportFiles)
                WriteLine(DirectoryColor, "Processed Directory {0} ({1} files, {2:N2} MB)", e.DirectoryData.Path, e.DirectoryData.FileCount, e.DirectoryData.TotalSize.GetMB());
            else
                WriteLine(DirectoryColor, "Done. ({0} files, {1:N2} MB)", e.DirectoryData.FileCount, e.DirectoryData.TotalSize.GetMB());
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
