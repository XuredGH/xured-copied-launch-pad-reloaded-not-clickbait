using System;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Attributes;

namespace LaunchpadReloaded.Components;

public class LaunchpadGameOptions
{
    public static LaunchpadGameOptions Instance { get; private set; }
    
    public CustomToggleOption FriendlyFire { get; }
    public CustomNumberOption CaptainMeetingCooldown { get; }
    public CustomNumberOption CaptainMeetingCount { get; }
    
    public LaunchpadGameOptions()
    {
        if (Instance != null)
        {
            throw new Exception("Can't have more than one Launchpad Options");
        }

        FriendlyFire = new CustomToggleOption("Friendly Fire", false);

        CaptainMeetingCooldown = new CustomNumberOption("Captain Meeting Cooldown",
            45,
            0,
            120,
            5,
            NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        CaptainMeetingCount = new CustomNumberOption("Captain Meeting Uses",
            3,
            1,
            5,
            1,
            NumberSuffixes.None,
            role: typeof(CaptainRole));
        
        
        Instance = this;
    }
}