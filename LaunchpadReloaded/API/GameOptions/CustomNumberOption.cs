using BepInEx.Configuration;
using System;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomNumberOption : AbstractGameOption
{
    public float Value { get; private set; }
    public float Min { get; }
    public float Max { get; }
    public float Increment { get; }
    public NumberSuffixes SuffixType { get; }
    public string NumberFormat { get; }
    public float Default { get; }
    public ConfigEntry<float> Config { get; }
    public Action<float> ChangedEvent = null;

    public CustomNumberOption(string title, float defaultValue, float min, float max, float increment, NumberSuffixes suffixType, string numberFormat = "0", Type role = null, bool save = true) : base(title, role, save)
    {
        Value = defaultValue;
        Default = defaultValue;
        Min = min;
        Max = max;
        Increment = increment;
        SuffixType = suffixType;
        NumberFormat = numberFormat;
        if (Save)
        {
            Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Number Options", title, defaultValue);
        }

        CustomOptionsManager.CustomNumberOptions.Add(this);
        SetValue(Save ? Config.Value : defaultValue);
    }

    public void SetValue(float newValue)
    {
        newValue = Mathf.Clamp(newValue, Min, Max);

        if (Save)
        {
            Config.Value = newValue;
        }

        Value = newValue;

        var behaviour = (NumberOption)OptionBehaviour;
        if (behaviour)
        {
            behaviour.Value = Value;
        }

        ChangedEvent?.Invoke(Value);
    }
    
    public void CreateNumberOption(NumberOption numberOption)
    {
        numberOption.name = Title;
        numberOption.Title = StringName;
        numberOption.Value = Value;
        numberOption.Increment = Increment;
        numberOption.SuffixType = SuffixType;
        numberOption.FormatString = NumberFormat;
        numberOption.ValidRange = new FloatRange(Min, Max);
        numberOption.ZeroIsInfinity = false;
        numberOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        numberOption.OnEnable();
        OptionBehaviour = numberOption;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetFloat());
    }

}