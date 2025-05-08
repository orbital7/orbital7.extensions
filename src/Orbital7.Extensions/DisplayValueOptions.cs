namespace Orbital7.Extensions;

public class DisplayValueOptions
{
    // Currency options.
    public bool UseCurrencyForDecimals { get; set; } = true;

    public bool CurrencyAddSymbol { get; set; } = true;

    public bool CurrencyAddCommas { get; set; } = true;

    public bool CurrencyAddPlusIfPositive { get; set; } = false;

    public int CurrencyDecimalPlaces { get; set; } = 2;


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
