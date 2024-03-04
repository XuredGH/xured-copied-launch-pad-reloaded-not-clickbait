using System;
using LaunchpadReloaded.Components;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : CustomGameOption
{
    public bool Value { get; private set; }
    private bool OldValue { get; set; }
    private Delegate OnValChange { get;}

    public CustomToggleOption(string title, bool defaultValue) : base(title)
    {
        Value = defaultValue;
        OldValue = !defaultValue;
        CustomGameOptionsManager.CustomToggleOptions.Add(this);
    }
    
    public CustomToggleOption(object @object, string title, Delegate action) : base(@object, title)
    {
        Value = false;
        OldValue = true;
        OnValChange = action;
        CustomGameOptionsManager.CustomToggleOptions.Add(this);
    }

    public void SetValue(bool newValue)
    {
        OldValue = Value;
        Value = newValue;
        if (OptionObject is not null)
        {
            OnValChange.DynamicInvoke(OptionObject, Value);
            Debug.LogWarning(LaunchpadGameOptions.Instance.FriendlyFireOn);
        }
    }
    
    protected override void OnValueChanged(OptionBehaviour optionBehaviour)
    {
        SetValue(optionBehaviour.GetBool());
    }
}