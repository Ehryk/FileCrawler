using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Extensions
{
    public static class ObjectExtensions
    {
        public static int ToInt(this object o, int defaultValue = -1)
        {
            return ToNullableInt(o) ?? defaultValue;
        }
        public static int? ToNullableInt(this object o)
        {
            if (o is int)
                return (int)o;

            int i;
            return int.TryParse(o.ToString(), out i) ? i : (int?)null;
        }

        public static decimal ToDecimal(this object o)
        {
            return ToNullableDecimal(o) ?? 0;
        }
        public static decimal? ToNullableDecimal(this object o)
        {
            if (o is decimal)
                return (decimal)o;

            decimal d;
            return Decimal.TryParse(o.ToString(), out d) ? d : (decimal?)null;
        }

        public static DateTime ToDateTime(this object o)
        {
            return ToNullableDateTime(o) ?? DateTime.MinValue;
        }
        public static DateTime? ToNullableDateTime(this object o)
        {
            if (o is DateTime)
                return (DateTime)o;

            DateTime d;
            return DateTime.TryParse(o.ToString(), out d) ? d : (DateTime?)null;
        }

        public static bool ToBoolean(this object o)
        {
            return ToNullableBoolean(o) ?? false;
        }
        public static bool? ToNullableBoolean(this object o)
        {
            if (o is bool)
                return (bool)o;

            bool b;
            return Boolean.TryParse(o.ToString(), out b) ? b : (bool?)null;
        }
    }
}
