using System;
using BepInEx.Configuration;
using Reactor.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

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
    public Action<float> ChangedEvent { get; set; }

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
            try
            {
                Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Number Options", title, defaultValue);
            }
            catch (Exception e)
            {
                Logger<LaunchpadReloadedPlugin>.Error(e.ToString());
            }
        }

        CustomOptionsManager.CustomNumberOptions.Add(this);
        SetValue(Save ? Config.Value : defaultValue);
    }

    public void SetValue(float newValue)
    {
        newValue = Mathf.Clamp(newValue, Min, Max);

        if (Save)
        {
            try
            {
                Config.Value = newValue;

            }
            catch (Exception e)
            {
                Logger<LaunchpadReloadedPlugin>.Error(e.ToString());
            }
        }

        Value = newValue;

        var behaviour = (NumberOption)OptionBehaviour;
        if (behaviour)
        {
            behaviour.Value = Value;
        }

        ChangedEvent?.Invoke(Value);
    }

    public NumberOption CreateNumberOption(NumberOption original, Transform container)
    {
        var numberOption = Object.Instantiate(original, container);
        
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

        return numberOption;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetFloat());
    }

}