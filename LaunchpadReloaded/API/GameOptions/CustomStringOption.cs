using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Reactor.Localization.Utilities;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomStringOption : AbstractGameOption
{
    public string Value { get; private set; }
    public int Default { get; }
    public string[] Options { get; private set; }
    public ConfigEntry<int> Config { get; }
    public Action<int> ChangedEvent = null;
    public CustomStringOption(string title, int defaultValue, string[] options, Type role = null) : base(title, role)
    {
        Value = options[defaultValue];
        Options = options;
        Default = defaultValue;
        CustomOptionsManager.CustomStringOptions.Add(this);

        Config = LaunchpadReloadedPlugin.Instance.Config.Bind("String Options", title, defaultValue);
        SetValue(Config.Value);
    }

    public void SetValue(int newValue)
    {
        Config.Value = newValue;
        Value = Options[newValue];

        var behaviour = (StringOption)OptionBehaviour;
        if (behaviour) behaviour.Value = newValue;
    }

    public void SetValue(string newValue) => SetValue(Options.ToList().IndexOf(newValue));

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetInt());
        if (ChangedEvent != null) ChangedEvent(optionBehaviour.GetInt());
    }

    public void CreateStringOption(StringOption stringOption)
    {
        var values = new List<StringNames>();
        foreach (var val in Options)
        {
            values.Add(CustomStringName.CreateAndRegister(val));
        }

        stringOption.name = Title;
        stringOption.Title = StringName;
        stringOption.Value = Config.Value;
        stringOption.Values = values.ToArray();
        stringOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        stringOption.OnEnable();
        OptionBehaviour = stringOption;
    }
}