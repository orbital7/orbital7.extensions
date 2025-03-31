namespace Orbital7.Extensions;

public enum PhoneNumberFormat
{

    DashesOnly,

    PeriodsOnly,

    ParenthesisAndDashes,
}

public static class PhoneNumberHelper
{
    public static string GetRawPhoneNumber(
        string value,
        bool includePlus1Prefix = false)
    {
        string phoneNumber = value.NumbersOnly().PruneStart("+").PruneStart("1");
        if (includePlus1Prefix && !phoneNumber.StartsWith("+1"))
            phoneNumber = "+1" + phoneNumber;

        return phoneNumber;
    }

    public static string GetFormattedPhoneNumber(
        string value,
        PhoneNumberFormat format = PhoneNumberFormat.DashesOnly)
    {
        // TODO: Expand to include non-NorthAmerican phone numbers.

        // Convert to numbers only 
        var raw = GetRawPhoneNumber(value);
        if (raw.Length >= 10)
        {
            var template = "({0}) {1}-{2}";
            if (format == PhoneNumberFormat.DashesOnly)
                template = "{0}-{1}-{2}";
            else if (format == PhoneNumberFormat.PeriodsOnly)
                template = "{0}.{1}.{2}";

            var phoneNumber = string.Format(template,
                raw.Substring(0, 3),
                raw.Substring(3, 3),
                raw.Substring(6, 4));

            // Treat the remainder as the extension.
            if (raw.Length > 10)
                return string.Format("{0} x{1}", phoneNumber, raw.Substring(10, raw.Length - 10));
            else
                return phoneNumber;
        }
        else
        {
            return raw;
        }
    }
}
