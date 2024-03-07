using System;
using System.Linq;
using LaunchpadReloaded.Components;
using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : AbstractGameOption
{
    public float Value { get; private set; }
    public float MinValue { get; }
    public float MaxValue { get; }
    public float Increment { get; }
    public NumberSuffixes SuffixType { get; }
    public string NumberFormat { get; }

    public CustomNumberOption(string title, float defaultValue, float minValue, float maxValue, float increment, NumberSuffixes suffixType, string numberFormat="0") : base(title)
    {
        Value = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
        SuffixType = suffixType;
        NumberFormat = numberFormat;
        CustomOptionsManager.CustomNumberOptions.Add(this);
    }

    public void SetValue(float newValue)
    {
        Value = newValue;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        Value = Mathf.Clamp(optionBehaviour.GetFloat(), MinValue, MaxValue);
    }
}