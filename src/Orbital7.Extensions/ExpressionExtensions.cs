using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static TAttribute GetAttribute<TAttribute, TModel>(this Expression<Func<TModel, object>> propertyExpression)
            where TAttribute : Attribute
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            return memberInfo.GetAttribute<TAttribute>(false);
        }

        public static bool HasAttribute<TAttribute, TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
            where TAttribute : Attribute
        {
            return expression.HasAttribute(typeof(TAttribute));
        }

        public static bool HasAttribute<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression, Type attributeType)
        {
            if (expression.Body is MemberExpression property)
            {
                var results = property.Member.GetCustomAttributes(attributeType, false);
                return results.Length > 0;
            }

            return false;
        }

        public static string GetPropertyDisplayName<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            var attr = memberInfo.GetPropertyDisplayAttribute();
            if (attr != null && !String.IsNullOrEmpty(attr.Name))
                return attr.Name;

            return memberInfo.Name;
        }

        public static string GetPropertyDisplayShortName<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            var attr = memberInfo.GetPropertyDisplayAttribute();
            if (attr != null)
            {
                if (!String.IsNullOrEmpty(attr.ShortName))
                    return attr.ShortName;
                else if (!String.IsNullOrEmpty(attr.Name))
                    return attr.Name;
            }

            return memberInfo.Name;
        }

        public static string GetPropertyDisplayDescription<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var attr = GetPropertyDisplayAttribute<T>(propertyExpression);
            return attr?.Description;
        }

        public static DisplayAttribute GetPropertyDisplayAttribute<T>(this Expression<Func<T, object>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            return memberInfo.GetPropertyDisplayAttribute();
        }
    }
}
