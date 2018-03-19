using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class BooleanExtensions
    {
        public static string ToTrueFalse(this bool value)
        {
            if (value)
                return "True";
            else
                return "False";
        }

        public static string Totruefalse(this bool value)
        {
            return value.ToString().ToLower();
        }

        public static string ToYesNo(this bool value)
        {
            if (value)
                return "Yes";
            else
                return "No";
        }

        public static string ToOnOff(this bool value)
        {
            if (value)
                return "On";
            else
                return "Off";
        }

        public static string ToText(this bool value, string textTrue, string textFalse)
        {
            if (value)
                return textTrue;
            else
                return textFalse;
        }
    }
}
