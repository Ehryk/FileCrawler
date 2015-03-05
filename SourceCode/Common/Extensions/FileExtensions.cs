using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
