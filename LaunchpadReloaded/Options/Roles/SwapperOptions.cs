using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles;

public class SwapperOptions : AbstractOptionGroup
{
    public override string GroupName => "Swapper";

    public override Type AdvancedRole => typeof(SwapperRole);

    [ModdedNumberOption("Swap Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float SwapCooldown { get; set; } = 30;

    [ModdedNumberOption("Swap Uses", 0, 8, zeroInfinity: true)]
    public float SwapUses { get; set; } = 3;

    [ModdedToggleOption("Can Swap with Impostors")]
    public bool CanSwapImpostors { get; set; } = true;
}