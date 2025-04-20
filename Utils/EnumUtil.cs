using System.Reflection;
using System.Runtime.Serialization;

namespace ISC_ELIB_SERVER.Utils
{
    public static class EnumUtil
    {
        public static string GetEnumStringValue<T>(string enumName) where T : Enum
        {
            var field = typeof(T).GetField(enumName);
            if (field == null) return null;

            var attribute = field.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? enumName;
        }
    }
}
