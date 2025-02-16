using LaunchpadReloaded.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles.Crewmate;

public class CoronerOptions : AbstractOptionGroup
{
    public override string GroupName => "Coroner";

    public override Type AdvancedRole => typeof(CoronerRole);

    [ModdedNumberOption("Freeze Cooldown", 0, 50, 5, MiraNumberSuffixes.Seconds)]
    public float FreezeCooldown { get; set; } = 15;

    [ModdedNumberOption("Freeze Uses", 0, 10, zeroInfinity: true)]
    public float FreezeUses { get; set; } = 0;
}