namespace System;

public class DisplayValueOptions
{
    // Currency options.
    public bool UseCurrencyForDecimals { get; set; } = true;

    public bool ForCurrencyAddSymbol { get; set; } = true;

    public bool ForCurrencyAddCommas { get; set; } = true;

    public bool ForCurrencyAddPlusIfPositive { get; set; } = false;


    // Date/time options.
    public string DateTimeFormat { get; set; }

    public string TimeZoneId { get; set; }


    public DisplayValueOptions()
    {

    }
}
