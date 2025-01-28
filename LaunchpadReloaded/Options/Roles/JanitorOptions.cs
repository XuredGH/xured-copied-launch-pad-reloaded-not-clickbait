using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Options.Roles;

public class JanitorOptions : AbstractOptionGroup
{
    public override string GroupName => "Janitor";

    public override Type AdvancedRole => typeof(JanitorRole);
    
    [ModdedNumberOption("Hide Bodies Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float HideCooldown { get; set; } = 5f;
    
    [ModdedNumberOption("Hide Bodies Uses", 1, 10)]
    public float HideUses { get; set; } = 3;
    
    [ModdedToggleOption("Clean Instead Of Hide")]
    public bool CleanInsteadOfHide { get; set; } = false;
}