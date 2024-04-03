﻿using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Localization.Utilities;

namespace LaunchpadReloaded.API.GameOptions;

public abstract class AbstractGameOption
{
    public string Title { get; }
    public StringNames StringName { get; }
    public Type AdvancedRole { get; }
    public bool Save { get; }
    public bool ShowInHideNSeek { get; init; }
    public CustomOptionGroup Group { get; set; }
    public Func<bool> Hidden { get; init; }
    public OptionBehaviour OptionBehaviour { get; protected set; }
    public void ValueChanged(OptionBehaviour optionBehaviour)
    {
        OnValueChanged(optionBehaviour);
        CustomOptionsManager.SyncOptions();
    }

    protected abstract void OnValueChanged(OptionBehaviour optionBehaviour);

    protected AbstractGameOption(string title, Type roleType, bool save)
    {
        Title = title;
        StringName = CustomStringName.CreateAndRegister(Title);
        if (roleType is not null && roleType.IsAssignableTo(typeof(ICustomRole)))
        {
            AdvancedRole = roleType;
        }

        Save = save;
        Hidden = () => false;
        CustomOptionsManager.CustomOptions.Add(this);
    }
}