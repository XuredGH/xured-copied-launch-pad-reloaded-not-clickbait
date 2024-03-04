using System;
using LaunchpadReloaded.API.GameOptions;
using Reactor.Utilities.Attributes;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }
    
    public CustomToggleOption FriendlyFire { get; }
    
    
    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        Instance = this;
    }
}