using System;
using LaunchpadReloaded.API.GameOptions;
using Reactor.Utilities.Attributes;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }
    
    public CustomToggleOption FriendlyFire { get; }
    
    public CustomNumberOption TestNumber { get; }
    public CustomNumberOption TestNumber2 { get; }
    
    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        TestNumber = new CustomNumberOption("Test number", 15, 0, 60, 5, NumberSuffixes.Seconds);
        TestNumber2 = new CustomNumberOption("Test number", 12.5f, 10, 60, 2.5f, NumberSuffixes.Multiplier, "0.0");
        
        Instance = this;
    }
}