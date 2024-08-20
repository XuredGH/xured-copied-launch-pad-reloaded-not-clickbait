﻿using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Roles.Options;

public class DetectiveOptions : IModdedOptionGroup
{
    public string GroupName => "Detective";
    
    public Type AdvancedRole => typeof(DetectiveRole);
    
    [ModdedToggleOption("Hide Suspects")]
    public bool HideSuspects { get; set; } = false;
    
    [ModdedNumberOption("Footsteps Duration", 1, 10, 1, NumberSuffixes.Seconds)]
    public float FootstepsDuration { get; set; } = 3;
    
    [ModdedNumberOption("Instinct Duration", 3, 76, 3, NumberSuffixes.Seconds)]
    public float InstinctDuration { get; set; } = 10;
    
    [ModdedNumberOption("Instinct Uses", 1, 10)]
    public int InstinctUses { get; set; } = 3;
    
    [ModdedNumberOption("Instinct Cooldown", 0, 45, 1, NumberSuffixes.Seconds)]
    public float InstinctCooldown { get; set; } = 15;
    
}