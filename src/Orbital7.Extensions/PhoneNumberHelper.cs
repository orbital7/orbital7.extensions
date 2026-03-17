namespace Orbital7.Extensions;

public enum PhoneNumberFormat
{

    DashesOnly,

    PeriodsOnly,

    ParenthesisAndDashes,
}

public static class PhoneNumberHelper
{
    // TODO: Expand to include non-North-American phone numbers.

    public static string? ToRaw(
        string? phoneNumber,
        bool includePlus1Prefix = false)
    {
        if (phoneNumber.HasText())
        {
            var rawPhoneNumber = phoneNumber.NumbersOnly()?.PruneStart("1");
            if (rawPhoneNumber.HasText())
            {
                if (includePlus1Prefix && !rawPhoneNumber.StartsWith("+1"))
                {
                    rawPhoneNumber = "+1" + rawPhoneNumber;
                }

                return rawPhoneNumber;
            }
        }

        return null;
    }

    public static string? Format(
        string? phoneNumber,
        bool includePlus1Prefix = false,
        PhoneNumberFormat format = PhoneNumberFormat.DashesOnly)
    {
        const int US_PHONE_NUMBER_LENGTH = 10;

        var raw = ToRaw(
            phoneNumber,
            includePlus1Prefix: includePlus1Prefix);

        if (raw.HasText() && raw.Length >= US_PHONE_NUMBER_LENGTH)
        {
            var template = "({0}) {1}-{2}";
            if (format == PhoneNumberFormat.DashesOnly)
            {
                template = "{0}-{1}-{2}";
            }
            else if (format == PhoneNumberFormat.PeriodsOnly)
            {
                template = "{0}.{1}.{2}";
            }

            var formattedPhoneNumber = string.Format(template,
                raw.Substring(0, 3),
                raw.Substring(3, 3),
                raw.Substring(6, 4));

            // Treat the remainder as the extension.
            if (raw.Length > US_PHONE_NUMBER_LENGTH)
            {
                var extension = raw.Substring(
                    US_PHONE_NUMBER_LENGTH, 
                    raw.Length - US_PHONE_NUMBER_LENGTH);

                return $"{formattedPhoneNumber} x{extension}";
            }
            else
            {
                return formattedPhoneNumber;
            }
        }
        else
        {
            return raw;
        }
    }
}
