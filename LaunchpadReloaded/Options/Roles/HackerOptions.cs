using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class HackerOptions : IModdedOptionGroup
{
    public string GroupName => "Hacker";

    public Type AdvancedRole => typeof(HackerRole);
    
    [ModdedNumberOption("Hack Cooldown", 10, 300, 10, NumberSuffixes.Seconds)]
    public float HackCooldown { get; set; } = 60;
    
    [ModdedNumberOption("Hacks Per Game", 1, 8)]
    public float HackUses { get; set; } = 2;
    
    [ModdedNumberOption("Map Cooldown", 0, 40, 3, NumberSuffixes.Seconds)]
    public float MapCooldown { get; set; } = 10;
    
    [ModdedNumberOption("Map Duration", 1, 30, 3, NumberSuffixes.Seconds)]
    public float MapDuration { get; set; } = 3;
}