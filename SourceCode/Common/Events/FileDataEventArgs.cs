using System;
using Common.Objects;

namespace Common.Events
{
    public class FileDataEventArgs : EventArgs
    {
        public FileDataEventArgs(FileData pData, DirectoryData pParentDirectory)
        {
            FileData = pData;
            ParentDirectory = pParentDirectory;
        }

        public FileDataEventArgs(FileDataEventArgs e)
        {
            FileData = e.FileData;
            ParentDirectory = e.ParentDirectory;
        }

        public FileData FileData { get; private set; }
        public DirectoryData ParentDirectory { get; private set; }
    }
}
