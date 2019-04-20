using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static TAttribute GetAttribute<TModel, TProperty, TAttribute>(this Expression<Func<TModel, TProperty>> propertyExpression)
            where TAttribute : Attribute
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            return memberInfo.GetAttribute<TAttribute>(false);
        }

        public static bool HasAttribute<TModel, TProperty, TAttribute>(this Expression<Func<TModel, TProperty>> expression)
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

        public static string GetPropertyDisplayName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            var attr = memberInfo.GetPropertyDisplayAttribute();
            if (attr != null && !string.IsNullOrEmpty(attr.Name))
                return attr.Name;

            return memberInfo.Name;
        }

        public static string GetPropertyDisplayShortName<TModel, TProperty>(this Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            var attr = memberInfo.GetPropertyDisplayAttribute();
            if (attr != null)
            {
                if (!string.IsNullOrEmpty(attr.ShortName))
                    return attr.ShortName;
                else if (!string.IsNullOrEmpty(attr.Name))
                    return attr.Name;
            }

            return memberInfo.Name;
        }

        public static string GetPropertyDisplayDescription<TModel, TProperty>(this Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var attr = GetPropertyDisplayAttribute(propertyExpression);
            return attr?.Description;
        }

        public static DisplayAttribute GetPropertyDisplayAttribute<TModel, TProperty>(this Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var memberInfo = propertyExpression.Body.GetPropertyInformation();
            return memberInfo.GetPropertyDisplayAttribute();
        }
    }
}
