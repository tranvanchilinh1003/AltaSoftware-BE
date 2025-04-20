using System;
using System.Globalization;

namespace ISC_ELIB_SERVER.Utils
{
    public static class ParsingUtil
    {
        public static bool TryParseInt(string? input, out int result) => int.TryParse(input, out result);

        public static bool TryParseLong(string? input, out long result) => long.TryParse(input, out result);

        public static bool TryParseDouble(string? input, out double result) => double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        public static bool TryParseFloat(string? input, out float result) => float.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        public static bool TryParseBool(string? input, out bool result) => bool.TryParse(input, out result);

        public static bool TryParseDateTime(string? input, out DateTime result, string format = "yyyy-MM-dd") => DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        public static bool TryParseTimestamp(string? input, out DateTime result, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (string.IsNullOrEmpty(input))
            {
                result = default;
                return false;
            }
            if (input.Contains("+") || input.Contains("-"))
            {
                string timestampWithTimezoneFormat = "yyyy-MM-dd HH:mm:sszzz";  
                return DateTime.TryParseExact(input, timestampWithTimezoneFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result);
            }
            else
            {
                return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }
        }
    }
}
