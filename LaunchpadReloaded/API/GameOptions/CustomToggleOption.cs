using BepInEx.Configuration;
using System;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : AbstractGameOption
{
    public bool Value { get; private set; }
    public bool Default { get; }
    public ConfigEntry<bool> Config { get; }
    public Action<bool> ChangedEvent = null;
    public CustomToggleOption(string title, bool defaultValue, Type role = null, bool save = true) : base(title, role, save)
    {
        Default = defaultValue;
        if (Save) Config = LaunchpadReloadedPlugin.Instance.Config.Bind("Toggle Options", title, defaultValue);
        CustomOptionsManager.CustomToggleOptions.Add(this);
        SetValue(Save ? Config.Value : defaultValue);
    }

    public void SetValue(bool newValue)
    {
        if (Save) Config.Value = newValue;
        Value = newValue;

        ToggleOption behaviour = (ToggleOption)OptionBehaviour;
        if (behaviour) behaviour.CheckMark.enabled = newValue;
        if (ChangedEvent != null) ChangedEvent(newValue);
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetBool());
    }

    public void CreateToggleOption(ToggleOption toggleOption)
    {
        toggleOption.name = Title;
        toggleOption.Title = StringName;
        toggleOption.CheckMark.enabled = Value;
        toggleOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        toggleOption.OnEnable();
        OptionBehaviour = toggleOption;
    }
}