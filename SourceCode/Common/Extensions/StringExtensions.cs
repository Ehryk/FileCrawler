using System;
using System.Xml;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Allows case insensitive checks
        /// </summary>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool EqualsIgnoreCase(this string source, string toCheck)
        {
            return source.Equals(toCheck, StringComparison.InvariantCultureIgnoreCase);
        }

        public static int ToInt(this string s)
        {
            return ToNullableInt(s) ?? -1;
        }
        public static int? ToNullableInt(this string s)
        {
            int i;
            return int.TryParse(s, out i) ? i : (int?)null;
        }

        public static decimal ToDecimal(this string s)
        {
            return ToNullableDecimal(s) ?? 0;
        }
        public static decimal? ToNullableDecimal(this string s)
        {
            decimal d;
            return Decimal.TryParse(s, out d) ? d : (decimal?)null;
        }

        public static DateTime ToDateTime(this string s)
        {
            return ToNullableDateTime(s) ?? DateTime.MinValue;
        }
        public static DateTime? ToNullableDateTime(this string s)
        {
            DateTime d;
            return DateTime.TryParse(s, out d) ? d : (DateTime?)null;
        }

        public static bool ToBoolean(this string s)
        {
            return ToNullableBoolean(s) ?? false;
        }
        public static bool? ToNullableBoolean(this string s)
        {
            bool b;
            return Boolean.TryParse(s, out b) ? b : (bool?)null;
        }

        public static XmlElement ToXmlElement(this string s)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(s);
            return document.DocumentElement;
        }
    }
}
