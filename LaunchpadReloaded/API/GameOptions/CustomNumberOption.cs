using System;
using BepInEx.Configuration;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : AbstractGameOption
{
    public float Value { get; private set; }
    public FloatRange Range { get; }
    public float Increment { get; }
    public NumberSuffixes SuffixType { get; }
    public string NumberFormat { get; }
    public float Default { get; }
    public ConfigEntry<float> Config { get; }
    public Action<float> ChangedEvent = null;

    public CustomNumberOption(string title, float defaultValue, FloatRange range, float increment, NumberSuffixes suffixType, string numberFormat = "0", Type role = null) : base(title, role)
    {
        Value = defaultValue;
        Default = defaultValue;
        Range = range;
        Increment = increment;
        SuffixType = suffixType;
        NumberFormat = numberFormat;
        Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Number Options", title, defaultValue);
        CustomOptionsManager.CustomNumberOptions.Add(this);
        SetValue(Config.Value);
    }

    public void SetValue(float newValue)
    {
        Config.Value = newValue;
        Value = newValue;

        var behaviour = (NumberOption)OptionBehaviour;
        if (behaviour)
        {
            behaviour.Value = Value;
        }
    }

    public void CreateNumberOption(NumberOption numberOption)
    {
        numberOption.name = Title;
        numberOption.Title = StringName;
        numberOption.Value = Config.Value;
        numberOption.Increment = Increment;
        numberOption.SuffixType = SuffixType;
        numberOption.FormatString = NumberFormat;
        numberOption.ZeroIsInfinity = false;
        numberOption.ValidRange = Range;
        numberOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        numberOption.OnEnable();
        OptionBehaviour = numberOption;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        var value = Mathf.Clamp(optionBehaviour.GetFloat(), Range.min, Range.max);
        SetValue(value);
        if (ChangedEvent != null)
        {
            ChangedEvent(value);
        }
    }

}