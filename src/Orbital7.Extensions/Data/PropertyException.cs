﻿namespace Orbital7.Extensions.Data;

public class PropertyException : 
    Exception
{
    public string PropertyName { get; set; }

    public PropertyException(string propertyName, string message)
        : base(message)
    {
        PropertyName = propertyName;
    }
}
