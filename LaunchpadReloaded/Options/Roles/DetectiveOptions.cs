using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Options.Roles;

public class DetectiveOptions : AbstractOptionGroup
{
    public override string GroupName => "Detective";
    
    public override Type AdvancedRole => typeof(DetectiveRole);
    
    [ModdedToggleOption("Hide Suspects")]
    public bool HideSuspects { get; set; } = false;
    
    [ModdedNumberOption("Footsteps Duration", 1, 10, 1, MiraNumberSuffixes.Seconds)]
    public float FootstepsDuration { get; set; } = 3;
    
    [ModdedNumberOption("Instinct Duration", 3, 76, 3, MiraNumberSuffixes.Seconds)]
    public float InstinctDuration { get; set; } = 10;
    
    [ModdedNumberOption("Instinct Uses", 1, 10)]
    public float InstinctUses { get; set; } = 3;
    
    [ModdedNumberOption("Instinct Cooldown", 0, 45, 1, MiraNumberSuffixes.Seconds)]
    public float InstinctCooldown { get; set; } = 15;
    
}