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

        public static string FixPath(string path)
        {
            //Fix to allow windows drive letters (C:, D:, etc.) to be a valid path
            path = path.Trim();
            if (path.EndsWith(":") && path.Length == 2)
                path = path + '\\';
            return path;
        }
    }
}
