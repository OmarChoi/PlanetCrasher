using System;

public readonly struct Currency
{
    public readonly double Value;
    public Currency(double value)
    {
        if (value < 0)
        {
            throw new Exception("[Currency.cs] Currency Value cannot be negative");
        }
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToFormattedString();
    }

    public static Currency operator +(Currency currency1, Currency currency2)
    {
        return new Currency(currency1.Value + currency2.Value);
    }
    
    public static Currency operator -(Currency a, Currency b)
    {
        return new Currency(a.Value - b.Value);
    }
    
    public static bool operator >=(Currency a, Currency b)
    {
        return a.Value >= b.Value;
    }

    public static bool operator <=(Currency a, Currency b)
    {
        return a.Value <= b.Value;
    }

    public static bool operator >(Currency a, Currency b)
    {
        return a.Value > b.Value;
    }

    public static bool operator <(Currency a, Currency b)
    {
        return a.Value < b.Value;
    }
    
    public static implicit operator Currency(double value)
    {
        return new Currency(value);
    }
    
    public static explicit operator double(Currency currency)
    {
        return currency.Value;
    }
}