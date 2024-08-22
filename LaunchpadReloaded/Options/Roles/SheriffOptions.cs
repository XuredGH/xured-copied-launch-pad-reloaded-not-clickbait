using System;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Roles.Options;

public class SheriffOptions : IModdedOptionGroup
{
    public string GroupName => "Sheriff";

    public Type AdvancedRole => typeof(SheriffRole);

    [ModdedNumberOption("Shot Cooldown", 0, 120, 5, NumberSuffixes.Seconds)]
    public float ShotCooldown { get; set; } = 45;

    [ModdedNumberOption("Shots Per Game", 1, 10)]
    public float ShotsPerGame { get; set; } = 3;

    [ModdedToggleOption("Should Crewmate Die")]
    public bool ShouldCrewmateDie { get; set; } = false;
}