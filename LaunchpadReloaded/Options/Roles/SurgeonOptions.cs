﻿using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles;

public class SurgeonOptions : AbstractOptionGroup
{
    public override string GroupName => "Surgeon";

    public override Type AdvancedRole => typeof(SurgeonRole);

    [ModdedNumberOption("Inject Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float InjectCooldown { get; set; } = 10f;

    [ModdedNumberOption("Inject Uses", 0, 10, zeroInfinity: true)]
    public float InjectUses { get; set; } = 0;

    [ModdedNumberOption("Poison Death Delay", 5, 60, 5, MiraNumberSuffixes.Seconds)]
    public float PoisonDelay { get; set; } = 10;

    [ModdedToggleOption("Can Use Standard Kill")]
    public bool StandardKill { get; set; } = false;
}