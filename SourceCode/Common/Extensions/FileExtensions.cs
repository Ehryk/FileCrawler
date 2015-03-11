using System;
using System.IO;

namespace Common.Extensions
{
    public static class FileExtensions
    {
        public static bool IsDirectory(this string path)
        {
            return Path.GetDirectoryName(path).Equals(path, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
