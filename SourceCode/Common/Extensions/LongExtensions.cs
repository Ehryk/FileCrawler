using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class LongExtensions
    {
        public static double GetKB(this long size)
        {
            return ((double)size) / 1024;
        }

        public static double GetMB(this long size)
        {
            return ((double)size) / 1024 / 1024;
        }

        public static double GetGB(this long size)
        {
            return ((double)size) / 1024 / 1024 / 1024;
        }
    }
}
