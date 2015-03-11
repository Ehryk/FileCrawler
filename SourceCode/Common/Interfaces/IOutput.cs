using System;
using System.Collections.Generic;
using System.Linq;
using Common.Events;
using System.IO;

namespace Common.Interfaces
{
    public interface IOutput
    {
        string Name();

        bool UsesCrawlStart();
        void CrawlStart(object sender, EventArgs e);

        bool UsesDirectoryFound();
        void DirectoryFound(object sender, DirectoryDataEventArgs e);

        bool UsesFileFound();
        void FileFound(object sender, FileDataEventArgs e);

        bool UsesFileProcessed();
        void FileProcessed(object sender, FileDataEventArgs e);

        bool UsesDirectoryProcessed();
        void DirectoryProcessed(object sender, DirectoryDataEventArgs e);

        bool UsesCrawlError();
        void CrawlError(object sender, ErrorEventArgs e);

        bool UsesFileInaccessible();
        void FileInaccessible(object sender, InaccessibleEventArgs e);

        bool UsesDirectoryInaccessible();
        void DirectoryInaccessible(object sender, InaccessibleEventArgs e);

        bool UsesCrawlEnd();
        void CrawlEnd(object sender, EventArgs e);
    }
}
