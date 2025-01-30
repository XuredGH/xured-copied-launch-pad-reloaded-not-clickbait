using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles;

public class JanitorOptions : AbstractOptionGroup
{
    public override string GroupName => "Janitor";

    public override Type AdvancedRole => typeof(JanitorRole);

    [ModdedNumberOption("Hide Bodies Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float HideCooldown { get; set; } = 5f;

    [ModdedNumberOption("Hide Bodies Uses", 0, 10, zeroInfinity: true)]
    public float HideUses { get; set; } = 3;

    [ModdedToggleOption("Clean Instead Of Hide")]
    public bool CleanInsteadOfHide { get; set; } = false;
}