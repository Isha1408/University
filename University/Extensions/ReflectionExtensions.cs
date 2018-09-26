using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyValue<T>(this T role, string propertyName)
        {
            return role.GetType().GetProperty(propertyName).GetValue(role, null).ToString();
        }
    }
}
