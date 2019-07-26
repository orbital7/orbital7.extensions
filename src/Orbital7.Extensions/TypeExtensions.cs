using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class TypeExtensions
    {
        public static bool IsNumeric(this Type type)
        {
            return type == typeof(int) || type == typeof(int?) ||
                   type == typeof(long) || type == typeof(long?) ||
                   type == typeof(decimal) || type == typeof(decimal?) ||
                   type == typeof(float) || type == typeof(float?) ||
                   type == typeof(double) || type == typeof(double?) ||
                   type == typeof(short) || type == typeof(short?);
        }
    }
}
