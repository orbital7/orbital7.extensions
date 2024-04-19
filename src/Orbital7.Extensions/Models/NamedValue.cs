namespace System;

public class NamedValue<TValue>
{
    public string Name { get; set; }

    public TValue Value { get; set; }

    public NamedValue()
    {

    }

    public NamedValue(
        string name,
        TValue value)
    {
        this.Name = name;
        this.Value = value;
    }

    public override string ToString()
    {
        return $"{this.Name}: {this.Value}";
    }
}
