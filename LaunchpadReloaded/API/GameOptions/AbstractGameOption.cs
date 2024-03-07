using System;
using LaunchpadReloaded.API.Roles;
using Reactor.Localization.Utilities;

namespace LaunchpadReloaded.API.GameOptions;

public abstract class AbstractGameOption
{    
    public string Title { get; }
    public StringNames StringName { get; }
    public OptionBehaviour OptionBehaviour { get; set; }
    public Type AdvancedRole { get; set; }
    
    public void ValueChanged(OptionBehaviour optionBehaviour)
    {
        OnValueChanged(optionBehaviour);
        CustomOptionsManager.SyncOptions();
    }

    protected abstract void OnValueChanged(OptionBehaviour optionBehaviour);

    protected AbstractGameOption(string title, Type roleType)
    {
        Title = title;
        StringName = CustomStringName.CreateAndRegister(Title);
        if (roleType is not null && roleType.IsAssignableTo(typeof(ICustomRole)))
        {
            AdvancedRole = roleType;
        }
        
        CustomOptionsManager.CustomOptions.Add(this);
    }
}