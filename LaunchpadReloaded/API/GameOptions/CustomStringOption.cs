using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Reactor.Localization.Utilities;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomStringOption : AbstractGameOption
{
    public int IndexValue { get; private set; }
    public string Value => Options[IndexValue];
    public int Default { get; }
    public string[] Options { get; private set; }
    public ConfigEntry<int> Config { get; }
    public Action<int> ChangedEvent { get; set; }
    public CustomStringOption(string title, int defaultValue, string[] options, Type role = null, bool save = true) : base(title, role, save)
    {
        IndexValue = defaultValue;
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
        IndexValue = newValue;

        var behaviour = (StringOption)OptionBehaviour;
        if (behaviour)
        {
            behaviour.Value = newValue;
        }

        ChangedEvent?.Invoke(newValue);
    }

    public void SetValue(string newValue) => SetValue(Options.ToList().IndexOf(newValue));

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetInt());
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
        stringOption.Value = Options.ToList().IndexOf(Value);
        stringOption.Values = values.ToArray();
        stringOption.OnValueChanged = (Il2CppSystem.Action<OptionBehaviour>)ValueChanged;
        stringOption.OnEnable();
        OptionBehaviour = stringOption;
    }
}