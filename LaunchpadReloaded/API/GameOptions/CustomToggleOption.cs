﻿using System;
using LaunchpadReloaded.Components;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public class CustomToggleOption : AbstractGameOption
{
    public bool Value { get; private set; }

    public CustomToggleOption(string title, bool defaultValue, Type role=null, bool defaultShow = true) : base(title, role)
    {
        Value = defaultValue;
        CustomOptionsManager.CustomToggleOptions.Add(this);
        ToggleVisibility(defaultShow);
    }
    
    public void SetValue(bool newValue)
    {
        Value = newValue;
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
        OptionBehaviour.gameObject.SetActive(IsVisible());
    }
}