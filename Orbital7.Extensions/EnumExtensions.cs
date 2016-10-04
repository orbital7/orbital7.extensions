using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class EnumExtensions
    {
        public static string ToDisplayString(this Enum value, string nullValue = "")
        {
            return AttributesHelper.GetEnumDisplayName(value, nullValue);
        }
    }
}
