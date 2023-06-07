using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace System.ComponentModel
{
    // Source: http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
    //         https://github.com/brianlagunas/BindingEnumsInWpf
    public class EnumDisplayTypeConverter : EnumConverter
    {
        public EnumDisplayTypeConverter(Type type)
            : base(type)
        {
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    FieldInfo fi = value.GetType().GetField(value.ToString());
                    if (fi != null)
                    {
                        var attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
                        return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Name))) ? attributes[0].Name : value.ToString();
                    }
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
