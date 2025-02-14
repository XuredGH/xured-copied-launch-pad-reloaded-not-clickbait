using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles;

public class GamblerOptions : AbstractOptionGroup
{
    public override string GroupName => "Gambler";

    public override Type AdvancedRole => typeof(GamblerRole);

    [ModdedNumberOption("Gamble Cooldown", 0, 60, 25, MiraNumberSuffixes.Seconds)]
    public float GambleCooldown { get; set; } = 25;

    [ModdedNumberOption("Gamble Uses", 0, 10, zeroInfinity: true)]
    public float GambleUses { get; set; } = 0;
}