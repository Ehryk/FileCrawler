using System;
using Common.Objects;

namespace Common.Events
{
    public class FileDataEventArgs : EventArgs
    {
        public bool Processed { get; private set; }
        public FileData FileData { get; private set; }
        public DirectoryData ParentDirectory { get; private set; }

        public FileDataEventArgs(FileData pData, DirectoryData pParentDirectory = null, bool pProcessed = false)
        {
            Processed = pProcessed;
            FileData = pData;
            ParentDirectory = pParentDirectory;
        }

        public FileDataEventArgs(FileDataEventArgs e)
        {
            Processed = e.Processed;
            FileData = e.FileData;
            ParentDirectory = e.ParentDirectory;
        }
    }
}
