using Microsoft.AspNetCore.Components.Forms;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RAFormValidationState
{
    public const string CASCADING_PARAMETER_RAFormInput = "RAFormInput";
    public const string CASCADING_PARAMETER_RAChildInputValidationFacilitatorForToString = "RAChildInputValidationFacilitatorForToString";

    private readonly EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;

    public List<string> GeneralErrors { get; set; } = new List<string>();

    public List<(string, string)> FieldErrors { get; set; } = new List<(string, string)>();

    public bool IsValid => this.GeneralErrors.Count == 0 && this.FieldErrors.Count == 0;

    public RAFormValidationState(
        EditContext editContext)
    {
        _editContext = editContext;
        _validationMessageStore = new ValidationMessageStore(_editContext);
    }

    public void Validate(
        object inputModel)
    {
        // Clear.
        _validationMessageStore.Clear();
        this.GeneralErrors.Clear();
        this.FieldErrors.Clear();

        // Reset.
        // TODO: Does this need to be done every time?
        _validationMessageStore = new ValidationMessageStore(_editContext);

        // Validate.
        var result = ValidationHelper.Validate(inputModel);

        // Process.
        foreach (var item in result.Results)
        {
            if (item.MemberNames != null && item.MemberNames.Any())
            {
                foreach (var member in item.MemberNames)
                {
                    AddPropertyError(member, item.ErrorMessage);
                }
            }
            else
            {
                AddGeneralError(item.ErrorMessage);
            }
        }
    }

    public void AddPropertyError(
        string? propertyName,
        string? error)
    {
        if (error.HasText())
        {
            if (propertyName.HasText())
            {
                // TODO: Need to setup cascading parameter for Model in RAForm that get into RAValidationMessage, then use that
                // as the model to create the FieldIdentifier.
                _validationMessageStore.Add(
                    new FieldIdentifier(_editContext.Model, propertyName), //this.EditContext.Field(propertyName), 
                    error);

                this.FieldErrors.Add((propertyName, error));
            }
            else
            {
                AddGeneralError(error);
            }
        }
    }

    public void AddGeneralError(
        string? error)
    {
        if (error.HasText())
        {
            _validationMessageStore.Add(
                new FieldIdentifier(_editContext.Model, fieldName: string.Empty),
                error);

            this.GeneralErrors.Add(error);
        }
    }
}
