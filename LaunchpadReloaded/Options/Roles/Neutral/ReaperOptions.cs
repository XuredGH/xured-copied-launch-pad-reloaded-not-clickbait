using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles.Neutral;

public class ReaperOptions : AbstractOptionGroup
{
    public override string GroupName => "Reaper";

    public override Type AdvancedRole => typeof(ReaperRole);

    [ModdedNumberOption("Collections To Win", 2, 8)]
    public float SoulCollections { get; set; } = 3;

    [ModdedNumberOption("Collect Cooldown", 1, 60, 5, MiraNumberSuffixes.Seconds)]
    public float CollectCooldown { get; set; } = 15;
}