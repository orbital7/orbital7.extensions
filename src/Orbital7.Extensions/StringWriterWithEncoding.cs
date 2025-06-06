﻿namespace Orbital7.Extensions;

public class StringWriterWithEncoding : 
    StringWriter
{
    private Encoding? _encoding;

    public override Encoding Encoding => _encoding ?? base.Encoding;

    public StringWriterWithEncoding() :
        base() { }

    public StringWriterWithEncoding(
        IFormatProvider formatProvider) : 
        base(formatProvider) { }

    public StringWriterWithEncoding(
        StringBuilder sb) : 
        base(sb) { }

    public StringWriterWithEncoding(
        StringBuilder sb, 
        IFormatProvider formatProvider) : 
        base(
            sb, 
            formatProvider) { }


    public StringWriterWithEncoding(
        Encoding encoding) : 
        base()
    {
        _encoding = encoding;
    }

    public StringWriterWithEncoding(
        IFormatProvider formatProvider, 
        Encoding encoding)  : 
        base(formatProvider)
    {
        _encoding = encoding;
    }

    public StringWriterWithEncoding(
        StringBuilder sb, 
        Encoding encoding) : 
        base(sb)
    {
        _encoding = encoding;
    }

    public StringWriterWithEncoding(
        StringBuilder sb, 
        IFormatProvider formatProvider, 
        Encoding encoding) : 
        base(
            sb, 
            formatProvider)
    {
        _encoding = encoding;
    }
} 
