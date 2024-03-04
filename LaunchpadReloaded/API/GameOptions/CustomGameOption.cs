using System;
using LaunchpadReloaded.Networking;
using Reactor.Localization.Utilities;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

public abstract class CustomGameOption
{    
    public string Title { get; }
    public StringNames StringName { get; }
    public OptionBehaviour OptionBehaviour { get; set; }
    
    public void ValueChanged(OptionBehaviour optionBehaviour)
    {
        OnValueChanged(optionBehaviour);
        CustomGameOptionsManager.RpcSyncOptions();
    }

    protected abstract void OnValueChanged(OptionBehaviour optionBehaviour);

    protected CustomGameOption(string title)
    {
        Title = title;
        StringName = CustomStringName.CreateAndRegister(Title);
        
        CustomGameOptionsManager.CustomOptions.Add(this);
    }
}