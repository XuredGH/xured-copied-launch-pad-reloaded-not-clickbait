using LaunchpadReloaded.Networking;
using Reactor.Localization.Utilities;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

public abstract class CustomOption
{    
    public string Title { get; set; }
    public StringNames StringName { get; set; }
    public OptionBehaviour OptionBehaviour { get; set; }

    public virtual void ValueChanged(OptionBehaviour optionBehaviour)
    {
        OnValueChanged(optionBehaviour);
        CustomGameOptionsManager.RpcSyncOptions();
    }

    protected abstract void OnValueChanged(OptionBehaviour optionBehaviour);
    
    protected CustomOption(string title)
    {
        Title = title;
        StringName = CustomStringName.CreateAndRegister(Title);
        
        CustomGameOptionsManager.CustomOptions.Add(this);
    }
}