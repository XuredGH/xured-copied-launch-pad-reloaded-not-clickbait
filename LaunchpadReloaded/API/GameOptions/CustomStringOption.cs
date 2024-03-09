using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using LaunchpadReloaded.Components;
using Reactor.Localization.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomStringOption : AbstractGameOption
{
    public string Value { get; private set; }
    public string[] Options { get; private set; }
    private Action<int> _changedEvent;
    public CustomStringOption(string title, string[] options, Action<int> changedEvent = null, Type role = null) : base(title, role)
    {
        Value = options[0];
        Options = options;
        _changedEvent = changedEvent;
        CustomOptionsManager.CustomStringOptions.Add(this);
    }

    public void SetValue(int newValue)
    {
        Value = Options[newValue];
    }

    public void SetValue(string newValue)
    {
        Value = newValue;
    }

    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetInt());
        if (_changedEvent != null) _changedEvent.Invoke(optionBehaviour.GetInt());
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