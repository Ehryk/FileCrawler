using System;
using Common.Objects;

namespace Common.Events
{
    public class DirectoryDataEventArgs : EventArgs
    {
        public bool FilesProcessed { get; private set; }
        public bool SubdirectoriesProcessed { get; private set; }
        public DirectoryData DirectoryData { get; private set; }

        public DirectoryDataEventArgs(DirectoryData pData, bool pFilesProcessed = false, bool pSubdirectoriesProcessed = false)
        {
            FilesProcessed = pFilesProcessed;
            SubdirectoriesProcessed = pSubdirectoriesProcessed;
            DirectoryData = pData;
        }

        public DirectoryDataEventArgs(DirectoryDataEventArgs e)
        {
            FilesProcessed = e.FilesProcessed;
            SubdirectoriesProcessed = e.SubdirectoriesProcessed;
            DirectoryData = e.DirectoryData;
        }
    }
}
