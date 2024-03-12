using BepInEx.Configuration;
using System;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : AbstractGameOption
{
    public bool Value { get; private set; }
    public bool Default { get; }
    public ConfigEntry<bool> Config { get; }

    public CustomToggleOption(string title, bool defaultValue, Type role = null) : base(title, role)
    {
        Value = defaultValue;
        Default = defaultValue;
        Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Toggle Options", title, defaultValue);
        CustomOptionsManager.CustomToggleOptions.Add(this);
        SetValue(Config.Value);
    }

    public void SetValue(bool newValue)
    {
        Config.Value = newValue;
        Value = newValue;

        ToggleOption behaviour = (ToggleOption)OptionBehaviour;
        if (behaviour) behaviour.CheckMark.enabled = Value;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetBool());
    }

    public void CreateToggleOption(ToggleOption toggleOption)
    {
        toggleOption.name = Title;
        toggleOption.Title = StringName;
        toggleOption.CheckMark.enabled = Config.Value;
        toggleOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        toggleOption.OnEnable();
        OptionBehaviour = toggleOption;
    }
}