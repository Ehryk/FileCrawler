namespace Common.Extensions
{
    public static class LongExtensions
    {
        public static decimal GetKB(this long size)
        {
            return ((decimal)size) / 1024;
        }

        public static decimal GetMB(this long size)
        {
            return ((decimal)size) / 1024 / 1024;
        }

        public static decimal GetGB(this long size)
        {
            return ((decimal)size) / 1024 / 1024 / 1024;
        }
    }
}
