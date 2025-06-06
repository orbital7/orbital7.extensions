namespace Orbital7.Extensions;

public static class ReflectionExtensions
{
    public static DisplayAttribute? GetDisplayAttribute(
        this MemberInfo memberInfo)
    {
        return memberInfo.GetAttribute<DisplayAttribute>();
    }

    public static string GetDisplayName(
        this MemberInfo memberInfo)
    {
        return memberInfo.GetDisplayAttribute()?.Name ??
            memberInfo.Name.PascalCaseToPhrase() ??
            memberInfo.Name;
    }

    public static T? GetAttribute<T>(
        this MemberInfo member)
        where T : Attribute
    {
        return (T?)member
            .GetCustomAttributes(typeof(T), false)
            .SingleOrDefault();
    }

    public static T GetRequiredAttribute<T>(
        this MemberInfo member)
        where T : Attribute
    {
        var attribute = GetAttribute<T>(member);

        if (attribute == null)
        {
            throw new ArgumentException(
                $"The {typeof(T).Name} attribute must be defined on member {member.Name}");
        }

        return attribute;
    }

    public static bool HasAttribute<T>(
        this MemberInfo member)
        where T : Attribute
    {
        return member.GetAttribute<T>() != null;
    }

    public static List<Type> GetTypesWithAttribute<TAttribute>(
        this Assembly assembly,
        Type objectType)
        where TAttribute : Attribute
    {
        var types = assembly.GetTypes(objectType);
        var attributeType = typeof(TAttribute);

        return (from x in types
                let a = x.GetCustomAttribute(attributeType) as TAttribute
                where a != null
                select x).ToList();
    }

    public static List<Type> GetTypesWithAttribute<TObject, TAttribute>(
        this Assembly assembly)
        where TAttribute : Attribute
    {
        return assembly.GetTypesWithAttribute<TAttribute>(typeof(TObject));
    }

    public static List<(Type, TAttribute)> GetTypeAttributePairs<TAttribute>(
        this Assembly assembly,
        Type objectType,
        bool includeNullAttributes = false)
        where TAttribute : Attribute
    {
        var types = assembly.GetTypes(objectType);
        var attributeType = typeof(TAttribute);

        return (from x in types
                let a = x.GetCustomAttribute(attributeType) as TAttribute
                where includeNullAttributes || a != null
                select (x, a)).ToList();
    }

    public static MemberInfo? GetMemberInfo(
        this Expression expression)
    {
        MemberExpression? memberExpression = expression as MemberExpression;

        if (memberExpression == null)
        {
            UnaryExpression? unaryExpr = expression as UnaryExpression;
            if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
            {
                memberExpression = unaryExpr.Operand as MemberExpression;
            }
        }

        if (memberExpression != null && memberExpression.Member.MemberType == MemberTypes.Property)
        {
            return memberExpression.Member;
        }

        return null;
    }

    public static List<Type> GetTypes<T>(
        this Assembly assembly)
    {
        return assembly.GetTypes(typeof(T));
    }

    public static List<Type> GetTypes(
        this Assembly assembly, 
        Type baseType)
    {
        var types = new List<Type>();
        string targetInterface = baseType.ToString();

        foreach (var assemblyType in assembly.GetTypes())
        {
            if ((assemblyType.IsPublic) && (!assemblyType.IsAbstract))
            {
                if (assemblyType.IsSubclassOf(baseType) || assemblyType.Equals(baseType) || (assemblyType.GetInterface(targetInterface) != null))
                    types.Add(assemblyType);
            }
        }

        return types;
    }

    public static T? CreateInstance<T>(
        this Type type)
    {
        return (T?)Activator.CreateInstance(type);
    }

    public static T? CreateInstance<T>(
        this Type type, 
        object[]? parameters)
    {
        return (T?)Activator.CreateInstance(type, parameters);
    }

    
    public static List<T> CreateInstances<T>(
        this List<Type> types)
    {
        var instances = new List<T>();

        foreach (Type typeItem in types)
        {
            try
            {
                T? instance = typeItem.CreateInstance<T>();
                if (instance != null)
                {
                    instances.Add(instance);
                }
            }
            catch { }
        }

        return instances;
    }

    public static List<T> CreateInstances<T>(
        this Assembly assembly)
    {
        return assembly
            .GetTypes<T>()
            .CreateInstances<T>();
    }

    public static List<T> CreateInstances<T>(
        this Assembly assembly, 
        Type baseType)
    {
        return assembly
            .GetTypes(baseType)
            .CreateInstances<T>();
    }

    public static List<Type> GetExternalTypes(
        this Type baseType)
    {
        return baseType.GetExternalTypes(ReflectionHelper.GetExecutingAssemblyFolderPath());
    }

    public static List<Type> GetExternalTypes(
        this Type baseType, 
        string assembliesFolderPath)
    {
        List<Type> types = new List<Type>();

        foreach (string filePath in Directory.GetFiles(assembliesFolderPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(filePath);
                types.AddRange(assembly.GetTypes(baseType));
            }
            catch { }
        }

        return types;
    }

    public static string? GetDisplayValue<TValue>(
        this MemberInfo memberInfo,
        TValue? value,
        TimeConverter? timeConverter,
        DisplayValueOptions? options = null)
    {
        return CalculateDisplayValue(
            value,
            memberInfo.Name,
            memberInfo,
            timeConverter,
            options);
    }

    public static string? GetDisplayValue<TValue>(
        this TValue? value,
        TimeConverter? timeConverter,
        string? propertyName = null,
        DisplayValueOptions? options = null)
    {
        return CalculateDisplayValue(
            value,
            propertyName,
            null,
            timeConverter,
            options);
    }

    private static string? CalculateDisplayValue<TValue>(
        TValue? value,
        string? propertyName,
        MemberInfo? memberInfo,
        TimeConverter? timeConverter,
        DisplayValueOptions? displayValueOptions)
    {
        if (value != null)
        {
            var type = value.GetType();
            var options = displayValueOptions ?? new DisplayValueOptions();
            var dataTypeAttribute = memberInfo?.GetAttribute<DataTypeAttribute>();

            if (type.IsBaseOrNullableEnumType())
            {
                return (value as Enum)?.ToDisplayString();
            }
            else if (type == typeof(DateOnly) || type == typeof(DateOnly?))
            {
                var date = (DateOnly)(object)value;
                if (options.DateOnlyFormat.HasText())
                {
                    return date.ToString(options.DateOnlyFormat);
                }
                else
                {
                    return date.ToDateTime().ToShortDateString();
                }
            }
            else if (type == typeof(TimeOnly) || type == typeof(TimeOnly?))
            {
                var time = (TimeOnly)(object)value;
                if (options.TimeOnlyFormat.HasText())
                {
                    return time.ToString(options.TimeOnlyFormat);
                }
                else
                {
                    var dateTime = ToDateTime(
                            new DateTime(new DateOnly(), time),
                        propertyName,
                        timeConverter,
                        options);

                    return dateTime.ToShortTimeString();
                }
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                var dateTime = ToDateTime(
                    (DateTime)(object)value,
                    propertyName,
                    timeConverter,
                    options);

                if (options.DateTimeFormat.HasText())
                {
                    return dateTime.ToString(options.DateTimeFormat);
                }
                else
                {
                    return dateTime.ToDefaultDateTimeString();
                }
            }
            else if (type == typeof(TimeSpan) || type == typeof(TimeSpan?))
            {
                var timeSpan = (TimeSpan)(object)value;
                if (options.TimeSpanFormat.HasText())
                {
                    return timeSpan.ToString(options.TimeSpanFormat);
                }
                else
                {
                    return timeSpan.ToHoursMinutesSecondsString();
                }
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                return ((bool)(object)value).ToYesNo();
            }
            else if (options.UseCurrencyForDecimals &&
                (type == typeof(decimal) || type == typeof(decimal?)))
            {
                var currency = (decimal)(object)value;
                return currency.ToCurrencyString(options);
            }
            else if (options.ForNumbersAddPlusIfPositive && 
                type.IsBaseOrNullableNumericType())
            {
                return "+" + value.ToString();
            }
            else if (dataTypeAttribute?.DataType == DataType.Password)
            {
                return "0000000000000000".Mask();
            }
        }

        return value?.ToString();
    }

    private static DateTime ToDateTime(
        DateTime dateTime,
        string? propertyName,
        TimeConverter? timeConverter,
        DisplayValueOptions? displayValueOptions)
    {
        DateTime value;

        // TODO: Should we just always do this and assuem the date/time is in UTC?
        if (propertyName?.EndsWith("Utc") ?? false)
        {
            DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        if (timeConverter != null)
        {
            if (displayValueOptions != null && displayValueOptions.TimeZoneId.HasText())
            {
                value = timeConverter.ToDateTime(dateTime, displayValueOptions.TimeZoneId);
            }
            else
            {
                value = timeConverter.ToLocalDateTime(dateTime);
            }
        }
        else
        {
            if (displayValueOptions != null && displayValueOptions.TimeZoneId.HasText())
            {
                value = dateTime.UtcToTimeZone(
                    TimeZoneInfo.FindSystemTimeZoneById(displayValueOptions.TimeZoneId));
            }
            else
            {
                value = dateTime.ToLocalTime();
            }
        }

        return value;
    }
}
