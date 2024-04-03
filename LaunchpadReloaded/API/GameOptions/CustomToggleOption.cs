using System;
using BepInEx.Configuration;
using Reactor.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : AbstractGameOption
{
    public bool Value { get; private set; }
    public bool Default { get; }
    public ConfigEntry<bool> Config { get; }
    public Action<bool> ChangedEvent { get; init; }
    public CustomToggleOption(string title, bool defaultValue, Type role = null, bool save = true) : base(title, role, save)
    {
        Default = defaultValue;
        if (Save)
        {
            try
            {
                Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Toggle Options", title, defaultValue);
            }
            catch (Exception e)
            {
                Logger<LaunchpadReloadedPlugin>.Error(e.ToString());
            }
        }
        CustomOptionsManager.CustomToggleOptions.Add(this);
        SetValue(Save ? Config.Value : defaultValue);
    }

    public void SetValue(bool newValue)
    {
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

        var oldValue = Value;
        Value = newValue;

        var behaviour = (ToggleOption)OptionBehaviour;
        if (behaviour)
        {
            behaviour.CheckMark.enabled = newValue;
        }

        if (newValue != oldValue)
        {
            ChangedEvent?.Invoke(newValue);
        }
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetBool());
    }

    public ToggleOption CreateToggleOption(ToggleOption original, Transform container)
    {
        var toggleOption = Object.Instantiate(original, container);
        
        toggleOption.name = Title;
        toggleOption.Title = StringName;
        toggleOption.CheckMark.enabled = Value;
        toggleOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        toggleOption.OnEnable();
        OptionBehaviour = toggleOption;
        return toggleOption;
    }
}