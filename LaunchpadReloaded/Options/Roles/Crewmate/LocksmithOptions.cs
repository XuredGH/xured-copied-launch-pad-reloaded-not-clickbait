using LaunchpadReloaded.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles.Crewmate;

public class LocksmithOptions : AbstractOptionGroup
{
    public override string GroupName => "Locksmith";

    public override Type AdvancedRole => typeof(LocksmithRole);

    [ModdedNumberOption("Seal Vent Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float SealVentCooldown { get; set; } = 35;

    [ModdedNumberOption("Seal Vent Uses", 0, 10, zeroInfinity: true)]
    public float SealVentUses { get; set; } = 3;

    [ModdedToggleOption("Seal Reveals Bodies")]
    public bool SealReveal { get; set; } = true;
}