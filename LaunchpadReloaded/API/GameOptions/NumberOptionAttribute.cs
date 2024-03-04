using System;

namespace LaunchpadReloaded.API.GameOptions;

[AttributeUsage(AttributeTargets.Property)]
public class NumberOptionAttribute : Attribute
{
    public string Name { get; }
    public float MinValue { get; }
    public float MaxValue { get; }
    public float Increment { get; }
    public NumberSuffixes SuffixType { get; }

    public NumberOptionAttribute(string name, float minValue, float maxValue, float increment, NumberSuffixes suffixType)
    {
        Name = name;
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
        SuffixType = suffixType;
    }
    
}