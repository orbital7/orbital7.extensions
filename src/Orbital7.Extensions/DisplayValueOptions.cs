namespace Orbital7.Extensions;

public sealed class DisplayValueOptions
{
    // Currency options.
    public bool UseCurrencyForDecimals { get; set; } = true;

    public bool ForCurrencyAddSymbol { get; set; } = true;

    public bool ForCurrencyAddCommas { get; set; } = true;

    public bool ForCurrencyAddPlusIfPositive { get; set; } = false;

    public int ForCurrencyRoundToDecimalPlaces { get; set; } = 2;

    public MidpointRounding ForCurrencyUseRoundingMode { get; set; } = MidpointRounding.ToEven;


    // Percentage options.
    public bool ForPercentageAddCommas { get; set; } = true;

    public bool ForPercentageAddPlusIfPositive { get; set; } = false;

    public int ForPercentageRoundToDecimalPlaces { get; set; } = 2;

    public MidpointRounding ForPercentageUseRoundingMode { get; set; } = MidpointRounding.ToEven;


    // Number options.
    public bool ForNumbersAddCommas { get; set; } = false;

    public bool ForNumbersAddPlusIfPositive { get; set; } = false;

    public int? ForNumbersRoundToDecimalPlaces { get; set; } = null;

    public MidpointRounding ForNumbersUseRoundingMode { get; set; } = MidpointRounding.ToEven;


    // Date/time options.
    public string? ForDateTimeUseFormat { get; set; }

    public string? ForDateOnlyUseFormat { get; set; }

    public string? ForTimeOnlyUseFormat { get; set; }

    public string? ForTimeSpanUseFormat { get; set; }

    public string? ForDateTimeUseTimeZoneId { get; set; }


    public DisplayValueOptions()
    {

    }
}
