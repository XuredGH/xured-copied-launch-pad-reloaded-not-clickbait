using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Options.Roles;

public class HackerOptions : AbstractOptionGroup
{
    public override string GroupName => "Hacker";

    public override Type AdvancedRole => typeof(HackerRole);
    
    [ModdedNumberOption("Hack Cooldown", 10, 300, 10, MiraNumberSuffixes.Seconds)]
    public float HackCooldown { get; set; } = 60;
    
    [ModdedNumberOption("Hacks Per Game", 1, 8)]
    public float HackUses { get; set; } = 2;
    
    [ModdedNumberOption("Map Cooldown", 0, 40, 3, MiraNumberSuffixes.Seconds)]
    public float MapCooldown { get; set; } = 10;
    
    [ModdedNumberOption("Map Duration", 1, 30, 3, MiraNumberSuffixes.Seconds)]
    public float MapDuration { get; set; } = 3;
}