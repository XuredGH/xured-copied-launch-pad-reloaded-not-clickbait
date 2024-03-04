using System;
using LaunchpadReloaded.API.GameOptions;
using Reactor.Utilities.Attributes;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }
    
    [ToggleOption("Friendly Fire")] public bool FriendlyFireOn { get; set; }
    
    [NumberOption("Test",0,10,1,NumberSuffixes.Seconds)] public float Test { get; set; }
    
    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        

        Instance = this;
    }
}