namespace Orbital7.Extensions.AspNetCore.RapidApp;

public static class Extensions
{
    public static ModalParameters ToModalParameters(
        this List<(string, object)> parameters)
    {
        var modalParameters = new ModalParameters();
        
        foreach (var parameter in parameters)
        {
            modalParameters.Add(parameter.Item1, parameter.Item2);
        }

        return modalParameters;
    }

    public static ModalParameters ToModalParameters(
        this (string, object) parameter)
    {
        return new List<(string, object)>()
        {
            parameter,
        }.ToModalParameters();
    }

    public static RAEditorType GetEditorType<TProperty>(
        this Expression<Func<TProperty>> expression)
    {
        var type = typeof(TProperty);
        //var fieldName = expression.Body.GetPropertyInformation()?.Name;
        var dataTypeAttribute = expression.GetAttribute<TProperty, DataTypeAttribute>();

        if (type == typeof(bool))
        {
            return RAEditorType.Switch;
        }
        else if (type == typeof(DateOnly) || 
            type == typeof(DateOnly?))
        {
            return RAEditorType.Date;
        }
        else if (type == typeof(Guid) || 
            type == typeof(Guid?))
        {
            return RAEditorType.SelectList;
        }
        else if (type.IsBaseOrNullableNumericType())
        {
            return RAEditorType.Number;
        }
        else if (expression.HasAttribute<TProperty, PhoneAttribute>() ||
            dataTypeAttribute?.DataType == DataType.PhoneNumber)
        {
            return RAEditorType.PhoneNumber;
        }
        else if (dataTypeAttribute?.DataType == DataType.PostalCode)
        {
            return RAEditorType.ZipCode;
        }
        else if (dataTypeAttribute?.DataType == DataType.MultilineText)
        {
            return RAEditorType.TextMultiLine;
        }
        else if (dataTypeAttribute?.DataType == DataType.Password)
        {
            return RAEditorType.Password;
        }
        //else if (type == typeof(IFormFile))
        //{
        //    return RAEditorType.File;
        //}
        else
        {
            return RAEditorType.Text;
        }
    }

    public static bool UsesInputTextEditor(
        this RAEditorType editorType)
    {
        switch (editorType)
        {
            case RAEditorType.Text:
            case RAEditorType.PhoneNumber:
            case RAEditorType.Email:
            case RAEditorType.ZipCode:
            case RAEditorType.Password:
                return true;

            default:
                return false;
        }
    }
}
