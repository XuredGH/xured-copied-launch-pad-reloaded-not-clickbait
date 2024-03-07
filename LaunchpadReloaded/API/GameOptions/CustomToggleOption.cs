using System;
using LaunchpadReloaded.Components;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : AbstractGameOption
{
    public bool Value { get; private set; }

    public CustomToggleOption(string title, bool defaultValue) : base(title)
    {
        Value = defaultValue;
        CustomOptionsManager.CustomToggleOptions.Add(this);
    }
    
    public void SetValue(bool newValue)
    {
        Value = newValue;
    }
    
    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetBool());
    }
}