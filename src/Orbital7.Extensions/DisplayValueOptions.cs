namespace Orbital7.Extensions;

public class DisplayValueOptions
{
    // Currency options.
    public bool UseCurrencyForDecimals { get; set; } = true;

    public bool ForCurrencyAddSymbol { get; set; } = true;

    public bool ForCurrencyAddCommas { get; set; } = true;

    public bool ForCurrencyAddPlusIfPositive { get; set; } = false;


    // Number options.
    public bool ForNumbersAddPlusIfPositive { get; set; } = false;


    // Date/time options.
    public string? DateTimeFormat { get; set; }

    public string? DateOnlyFormat { get; set; }

    public string? TimeOnlyFormat { get; set; }

    public string? TimeSpanFormat { get; set; }

    public string? TimeZoneId { get; set; }


    public DisplayValueOptions()
    {

    }
}
