namespace Orbital7.Extensions;

public sealed class DisplayValueOptionsBuilder
{
    private DisplayValueOptions _options = new DisplayValueOptions();

    public DisplayValueOptionsBuilder()
    {

    }

    internal DisplayValueOptionsBuilder(
        DisplayValueOptions options)
    {
        _options = options;
    }

    public DisplayValueOptionsBuilder Clone()
    {
        return new DisplayValueOptionsBuilder(
            _options.CloneIgnoringReferenceProperties());
    }

    public DisplayValueOptions Build()
    {
        return _options;
    }


    // Currency options.
    public DisplayValueOptionsBuilder UseCurrencyForDecimals(
        bool value)
    {
        _options.UseCurrencyForDecimals = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForCurrencyAddSymbol(
        bool value)
    {
        _options.ForCurrencyAddSymbol = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForCurrencyAddCommas(
        bool value)
    {
        _options.ForCurrencyAddCommas = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForCurrencyAddPlusIfPositive(
        bool value)
    {
        _options.ForCurrencyAddPlusIfPositive = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForCurrencyRoundToDecimalPlaces(
        int value)
    {
        _options.ForCurrencyRoundToDecimalPlaces = value;
        return this;
    }
    public DisplayValueOptionsBuilder ForCurrencyUseRoundingMode(
        MidpointRounding value)
    {
        _options.ForCurrencyUseRoundingMode = value;
        return this;
    }


    // Percentage options.
    public DisplayValueOptionsBuilder ForPercentageAddCommas(
        bool value)
    {
        _options.ForPercentageAddCommas = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForPercentageAddPlusIfPositive(
        bool value)
    {
        _options.ForPercentageAddPlusIfPositive = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForPercentageRoundToDecimalPlaces(
        int value)
    {
        _options.ForPercentageRoundToDecimalPlaces = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForPercentageUseRoundingMode(
        MidpointRounding value)
    {
        _options.ForPercentageUseRoundingMode = value;
        return this;
    }


    // Number options.
    public DisplayValueOptionsBuilder ForNumbersAddCommas(
        bool value)
    {
        _options.ForNumbersAddCommas = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForNumbersAddPlusIfPositive(
        bool value)
    {
        _options.ForNumbersAddPlusIfPositive = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForNumbersRoundToDecimalPlaces(
        int? value)
    {
        _options.ForNumbersRoundToDecimalPlaces = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForNumbersUseRoundingMode(
        MidpointRounding value)
    {
        _options.ForNumbersUseRoundingMode = value;
        return this;
    }


    // Date/time options.
    public DisplayValueOptionsBuilder ForDateTimeUseFormat(
        string value)
    {
        _options.ForDateTimeUseFormat = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForDateOnlyUseFormat(
        string value)
    {
        _options.ForDateOnlyUseFormat = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForTimeOnlyUseFormat(
        string value)
    {
        _options.ForTimeOnlyUseFormat = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForTimeSpanUseFormat(
        string value)
    {
        _options.ForTimeSpanUseFormat = value;
        return this;
    }

    public DisplayValueOptionsBuilder ForDateTimeUseTimeZoneId(
        string value)
    {
        _options.ForDateTimeUseTimeZoneId = value;
        return this;
    }
}