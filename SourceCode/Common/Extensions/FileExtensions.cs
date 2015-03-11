using System;
using System.IO;
using Common.Objects;

namespace Common.Extensions
{
    public static class FileExtensions
    {
        public static bool IsDirectory(this string path)
        {
            return Path.GetDirectoryName(path).Equals(path, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool HasAttribute(this DirectoryData data, FileAttributes value)
        {
            return data.Attributes.HasAttribute(value);
        }

        public static bool HasAttribute(this FileData data, FileAttributes value)
        {
            return data.Attributes.HasAttribute(value);
        }

        public static bool HasAttribute(this FileAttributes attributes, FileAttributes value)
        {
            return (attributes & value) == value;
        }
    }
}
