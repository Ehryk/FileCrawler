using System;
using System.IO;

namespace Common.Events
{
    public class InaccessibleEventArgs : EventArgs
    {
        public InaccessibleEventArgs(DirectoryInfo pInfo, Exception pException = null)
        {
            IsFile = false;
            IsDirectory = true;
            Path = pInfo.FullName;
            Exception = pException;
        }

        public InaccessibleEventArgs(FileInfo pInfo, Exception pException = null)
        {
            IsFile = true;
            IsDirectory = false;
            Path = pInfo.FullName;
            Exception = pException;
        }

        public InaccessibleEventArgs(InaccessibleEventArgs e)
        {
            IsFile = e.IsFile;
            IsDirectory = e.IsDirectory;
            Path = e.Path;
            Exception = e.Exception;
        }
        
        public bool IsFile { get; private set; }
        public bool IsDirectory { get; private set; }
        public string Path { get; private set; }
        public Exception Exception { get; private set; }
    }
}
