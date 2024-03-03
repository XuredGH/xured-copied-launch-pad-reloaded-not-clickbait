using System.Linq;
using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : CustomOption
{
    public float Value { get; private set; }

    public float OldValue { get; private set; }
    
    public float MinValue { get; }
    
    public float MaxValue { get; }
    
    public float Increment { get; }
    
    public NumberSuffixes SuffixType { get; }
    
    
    public CustomNumberOption(string title, float defaultValue, float minValue, float maxValue, float increment, NumberSuffixes suffixType) : base(title)
    {
        Value = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
        SuffixType = suffixType;
        
        CustomGameOptionsManager.CustomNumberOptions.Add(this);
    }

    public void SetValue(float newValue)
    {
        OldValue = Value;
        Value = newValue;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        OldValue = Value;
        Value = Mathf.Clamp(optionBehaviour.GetFloat(), MinValue, MaxValue);
        Debug.LogError(OldValue+", "+Value);
    }
}