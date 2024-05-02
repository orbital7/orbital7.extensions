using Microsoft.AspNetCore.Components.Forms;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RAFormValidationState
{
    private ValidationMessageStore ValidationMessageStore { get; set; }

    private EditContext EditContext { get; set; }

    public List<string> GeneralErrors { get; set; } = new List<string>();

    public List<(string, string)> FieldErrors { get; set; } = new List<(string, string)>();

    public bool IsValid => this.GeneralErrors.Count == 0 && this.FieldErrors.Count == 0;

    public RAFormValidationState(
        EditContext editContext)
    {
        this.EditContext = editContext;
        this.ValidationMessageStore = new ValidationMessageStore(this.EditContext);
    }

    public void Validate(
        object inputModel)
    {
        // Clear.
        this.ValidationMessageStore.Clear();
        this.GeneralErrors.Clear();
        this.FieldErrors.Clear();

        // Reset.
        // TODO: Does this need to be done every time?
        this.ValidationMessageStore = new ValidationMessageStore(this.EditContext);

        // Validate.
        var result = inputModel.Validate();

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

    //public void AddErrors(
    //    List<(string, string)> errors)
    //{
    //    foreach (var error in errors)
    //    {
    //        AddPropertyError(error.Item1, error.Item2);
    //    }
    //}

    public void AddPropertyError(
        string propertyName,
        string error)
    {
        if (propertyName.HasText())
        {
            this.ValidationMessageStore.Add(
                this.EditContext.Field(propertyName), error);

            this.FieldErrors.Add((propertyName, error));
        }
        else
        {
            AddGeneralError(error);
        }
    }

    public void AddGeneralError(
        string error)
    {
        this.ValidationMessageStore.Add(
            new FieldIdentifier(this.EditContext.Model, fieldName: string.Empty), 
            error);

        this.GeneralErrors.Add(error);
    }
}
