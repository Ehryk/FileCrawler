using System;
using Common.Objects;

namespace Common.Events
{
    public class DirectoryDataEventArgs : EventArgs
    {
        public DirectoryDataEventArgs(DirectoryData pData)
        {
            DirectoryData = pData;
        }

        public DirectoryDataEventArgs(DirectoryDataEventArgs e)
        {
            DirectoryData = e.DirectoryData;
        }

        public DirectoryData DirectoryData { get; private set; }
    }
}
