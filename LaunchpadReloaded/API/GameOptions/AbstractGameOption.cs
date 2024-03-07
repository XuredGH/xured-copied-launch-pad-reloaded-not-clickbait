using System;
using LaunchpadReloaded.Networking;
using Reactor.Localization.Utilities;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

public abstract class AbstractGameOption
{    
    public string Title { get; }
    public StringNames StringName { get; }
    public OptionBehaviour OptionBehaviour { get; set; }
    
    public void ValueChanged(OptionBehaviour optionBehaviour)
    {
        OnValueChanged(optionBehaviour);
        CustomOptionsManager.SyncOptions();
    }

    protected abstract void OnValueChanged(OptionBehaviour optionBehaviour);

    protected AbstractGameOption(string title)
    {
        Title = title;
        StringName = CustomStringName.CreateAndRegister(Title);
        
        CustomOptionsManager.CustomOptions.Add(this);
    }
}