using System;
using System.Linq;
using System.Reflection;

namespace Kk.LeoQuery
{
    internal class Utils
    {
        public static string FieldsToStringByReflection(object o)
        {
            Type type = o.GetType();
            string[] fieldValues = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Select(field => $"{field.Name}: {field.GetValue(o)}")
                .ToArray();
            return $"{string.Join(", ", fieldValues)}";
        }
    }
}