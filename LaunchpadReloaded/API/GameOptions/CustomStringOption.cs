using BepInEx.Configuration;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor.Localization.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomStringOption : AbstractGameOption
{
    public string Value { get; private set; }
    public int Default { get; }
    public string[] Options { get; private set; }
    public ConfigEntry<int> Config { get; }
    public Action<int> ChangedEvent { get; set; }
    public CustomStringOption(string title, int defaultValue, string[] options, Type role = null, bool save = true) : base(title, role, save)
    {
        Value = options[defaultValue];
        Options = options;
        Default = defaultValue;
        CustomOptionsManager.CustomStringOptions.Add(this);
        ChangedEvent = null;

        if (Save) Config = LaunchpadReloadedPlugin.Instance.Config.Bind("String Options", title, defaultValue);
        SetValue(Save ? Config.Value : defaultValue);
    }

    public void SetValue(int newValue)
    {
        if (Save) Config.Value = newValue;
        Value = Options[newValue];

        StringOption behaviour = (StringOption)OptionBehaviour;
        if (behaviour) behaviour.Value = newValue;
        if (ChangedEvent != null) ChangedEvent(newValue);
    }

    public void SetValue(string newValue) => SetValue(Options.ToList().IndexOf(newValue));

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetInt());
    }

    public void CreateStringOption(StringOption stringOption)
    {
        List<StringNames> values = new List<StringNames>();
        foreach (string val in Options)
        {
            values.Add(CustomStringName.CreateAndRegister(val));
        }

        stringOption.name = Title;
        stringOption.Title = StringName;
        stringOption.Value = Options.ToList().IndexOf(Value);
        stringOption.Values = (Il2CppStructArray<StringNames>)values.ToArray();
        stringOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        stringOption.OnEnable();
        OptionBehaviour = stringOption;
    }
}