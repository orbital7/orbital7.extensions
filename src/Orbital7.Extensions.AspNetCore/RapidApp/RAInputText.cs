using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

// TODO: This is a duplicate of InputText, but adding TValue. Ideally we 
// would just use InputText, but I haven't found a way to handle TValue with the binding.
//
// https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/InputText.cs
public class RAInputText<TValue> :
    InputBase<TValue>
{
    [DisallowNull]
    public ElementReference? Element { get; protected set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "input");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        AddAttributeIfNotNullOrEmpty(builder, 2, "name", NameAttributeValue);
        AddAttributeIfNotNullOrEmpty(builder, 3, "class", CssClass);
        builder.AddAttribute(4, "value", CurrentValueAsString);
        builder.AddAttribute(5, "onchange", EventCallback.Factory.CreateBinder<string>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
        builder.SetUpdatesAttributeName("value");
        builder.AddElementReferenceCapture(6, __inputReference => Element = __inputReference);
        builder.CloseElement();
    }

    // NOTE: This is apparently an internal ASP.NET Core extension method.
    private void AddAttributeIfNotNullOrEmpty(
        RenderTreeBuilder builder, 
        int sequence, 
        string name, 
        string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            #pragma warning disable ASP0006
            builder.AddAttribute(sequence, name, value);
        }
    }

    protected override bool TryParseValueFromString(
        string value,
        out TValue result,
        out string validationErrorMessage)
    {
        validationErrorMessage = null;

        // TODO: What other types do we need to support here?
        if (typeof(TValue) == typeof(string))
        {
            result = (TValue)(object)value;
            return true;
        }

        result = default;
        return true;
    }
}
