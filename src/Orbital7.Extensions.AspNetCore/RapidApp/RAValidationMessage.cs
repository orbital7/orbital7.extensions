using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

// NOTE: Source:
//  https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/ValidationMessage.cs
//
// If _fieldIdentifier was protected, we would be able to implement this by just overriding OnParametersSet,
// but since it isn't, we have to duplicate the original source code for a new class. The modification here
// is to change the way the field name gets set so that it handles complex objects with nested children.

/// <summary>
/// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
/// </summary>
public class RAValidationMessage<TValue> : ComponentBase, IDisposable
{
    private EditContext? _previousEditContext;
    private Expression<Func<TValue>>? _previousFieldAccessor;
    private readonly EventHandler<ValidationStateChangedEventArgs>? _validationStateChangedHandler;
    private FieldIdentifier _fieldIdentifier;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created <c>div</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter] EditContext CurrentEditContext { get; set; } = default!;

    // JVE: Added cascading RAFormInput parameter.
    [CascadingParameter(Name = RAFormValidationState.CASCADING_PARAMETER_RAFormInput)] public object? RAFormInput { get; set; }

    // JVE: Added cascading RAFormInputForToString parameter.
    [CascadingParameter(Name = RAFormValidationState.CASCADING_PARAMETER_RAChildInputValidationFacilitatorForToString)] public string? RAChildInputValidationFacilitatorForToString { get; set; }

    /// <summary>
    /// Specifies the field for which validation messages should be displayed.
    /// </summary>
    [Parameter] public Expression<Func<TValue>>? For { get; set; }

    /// <summary>`
    /// Constructs an instance of <see cref="ValidationMessage{TValue}"/>.
    /// </summary>
    public RAValidationMessage()
    {
        _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                $"an {nameof(EditForm)}.");
        }

        if (For == null) // Not possible except if you manually specify T
        {
            throw new InvalidOperationException($"{GetType()} requires a value for the " +
                $"{nameof(For)} parameter.");
        }
        else if (For != _previousFieldAccessor && this.RAFormInput != null)
        {
            // JVE: Here's the change we're making: use the input as the
            // model and parse out the full field name (including nested
            // prefix).
            //
            //_fieldIdentifier = FieldIdentifier.Create(For);
            var fieldName = ParseRAFieldName();
            if (this.RAChildInputValidationFacilitatorForToString.HasText())
            {
                fieldName = ParseRAFormChildInputPrefix() + "." + fieldName;
            }

            _fieldIdentifier = new FieldIdentifier(
                this.RAFormInput, 
                fieldName);

            _previousFieldAccessor = For;
        }

        if (CurrentEditContext != _previousEditContext)
        {
            DetachValidationStateChangedListener();
            CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            _previousEditContext = CurrentEditContext;
        }
    }

    private string ParseRAFieldName()
    {
        var fieldName = String.Empty;

        if (this.For != null)
        {
            var expressionToString = this.For.ToString();
            var propertyPath = expressionToString.Parse(").")[1];
            var propertyFrags = propertyPath.Parse(".");

            for (int i = 1; i < propertyFrags.Length; i++)
            {
                fieldName += propertyFrags[i] + ".";
            }
        }

        return fieldName.PruneEnd(".");
    }

    private string ParseRAFormChildInputPrefix()
    {
        var frags = this.RAChildInputValidationFacilitatorForToString.Parse(".");
        return frags.Last();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var messages = CurrentEditContext.GetValidationMessages(_fieldIdentifier).ToList();
        if (messages.Count == 0)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "validation-message");
            builder.AddMultipleAttributes(2, AdditionalAttributes);
            builder.AddMarkupContent(3, "&nbsp;");
            builder.CloseElement();
        }
        else
        {
            foreach (var message in messages)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "validation-message");
                builder.AddMultipleAttributes(2, AdditionalAttributes);
                builder.AddContent(3, message);
                builder.CloseElement();
            }
        }
    }

    /// <summary>
    /// Called to dispose this instance.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> if called within <see cref="IDisposable.Dispose"/>.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    void IDisposable.Dispose()
    {
        DetachValidationStateChangedListener();
        Dispose(disposing: true);
    }

    private void DetachValidationStateChangedListener()
    {
        if (_previousEditContext != null)
        {
            _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }
    }
}