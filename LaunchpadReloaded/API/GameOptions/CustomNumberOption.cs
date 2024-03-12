using System;
using System.Linq;
using LaunchpadReloaded.Components;
using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : AbstractGameOption
{
    public float Value { get; private set; }
    public FloatRange Range { get; }
    public float Increment { get; }
    public NumberSuffixes SuffixType { get; }
    public string NumberFormat { get; }

    public CustomNumberOption(string title, float defaultValue, FloatRange range, float increment, NumberSuffixes suffixType, string numberFormat="0", Type role=null) : base(title, role)
    {
        Value = defaultValue;
        Range = range;
        Increment = increment;
        SuffixType = suffixType;
        NumberFormat = numberFormat;
        CustomOptionsManager.CustomNumberOptions.Add(this);
    }

    public void SetValue(float newValue)
    {
        Value = newValue;
    }

    public void CreateNumberOption(NumberOption numberOption)
    {
        numberOption.name = Title;
        numberOption.Title = StringName;
        numberOption.Value = Value;    
        numberOption.Increment = Increment;
        numberOption.SuffixType = SuffixType;
        numberOption.FormatString = NumberFormat;
        numberOption.ValidRange = Range;
        numberOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        numberOption.OnEnable();
        OptionBehaviour = numberOption;
    }
    
    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        Value = Mathf.Clamp(optionBehaviour.GetFloat(), Range.min, Range.max);
    }
    
}