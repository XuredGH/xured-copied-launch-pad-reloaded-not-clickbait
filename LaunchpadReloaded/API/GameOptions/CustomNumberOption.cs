using System;
using System.Linq;
using LaunchpadReloaded.Components;
using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : CustomGameOption
{
    public float Value { get; private set; }

    private float OldValue { get; set; }
    
    public float MinValue { get; }
    
    public float MaxValue { get; }
    
    public float Increment { get; }
    
    public NumberSuffixes SuffixType { get; }
    
    private Delegate OnValChange { get; }
    
    
    public CustomNumberOption(string title, float defaultValue, float minValue, float maxValue, float increment, NumberSuffixes suffixType) : base(title)
    {
        Value = defaultValue;
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
        SuffixType = suffixType;
        
        CustomGameOptionsManager.CustomNumberOptions.Add(this);
    }
    
    public CustomNumberOption(object @object, Delegate action, NumberOptionAttribute attribute) : base(@object, attribute.Name)
    {
        Value = (attribute.MinValue+attribute.MaxValue)/2f;
        MinValue = attribute.MinValue;
        MaxValue = attribute.MaxValue;
        Increment = attribute.Increment;
        SuffixType = attribute.SuffixType;
        OnValChange = action;
        
        CustomGameOptionsManager.CustomNumberOptions.Add(this);
    }

    public void SetValue(float newValue)
    {
        OldValue = Value;
        Value = newValue;
        
            OnValChange.DynamicInvoke(OptionObject, Value);
            Debug.LogWarning(LaunchpadGameOptions.Instance.Test);
        
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        OldValue = Value;
        Value = Mathf.Clamp(optionBehaviour.GetFloat(), MinValue, MaxValue);
        Debug.LogError(OldValue+", "+Value);
    }
}