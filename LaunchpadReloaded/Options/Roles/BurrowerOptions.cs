﻿using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles;

public class BurrowerOptions : AbstractOptionGroup
{
    public override string GroupName => "Burrower";

    public override Type AdvancedRole => typeof(BurrowerRole);

    [ModdedNumberOption("Vent Dig Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float VentDigCooldown { get; set; } = 35;

    [ModdedNumberOption("Vent Dig Uses", 0, 12, 2, zeroInfinity: true)]
    public float VentDigUses { get; set; } = 0;

    [ModdedNumberOption("Min Vent Distance", 0, 10, 0.5f)]
    public float VentDist { get; set; } = 1.5f;
}