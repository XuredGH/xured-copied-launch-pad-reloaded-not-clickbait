using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class JanitorOptions : IModdedOptionGroup
{
    public string GroupName => "Janitor";

    public Type AdvancedRole => typeof(JanitorRole);
    
    [ModdedNumberOption("Hide Bodies Cooldown", 0, 120, 5, NumberSuffixes.Seconds)]
    public float HideCooldown { get; set; } = 5f;
    
    [ModdedNumberOption("Hide Bodies Uses", 1, 10)]
    public float HideUses { get; set; } = 3;
    
    [ModdedToggleOption("Clean Instead Of Hide")]
    public bool CleanInsteadOfHide { get; set; } = false;
}