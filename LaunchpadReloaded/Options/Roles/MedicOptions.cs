using System;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles;

public class MedicOptions : IModdedOptionGroup
{
    public string GroupName => "Medic";

    public Type AdvancedRole => typeof(MedicRole);

    [ModdedToggleOption("Only Allow Reviving in MedBay/Laboratory")]
    public bool OnlyAllowInMedbay { get; set; } = false;

    [ModdedToggleOption("Can Drag Bodies")]
    public bool DragBodies { get; set; } = false;

    [ModdedNumberOption("Max Revives", 1, 9)]
    public float MaxRevives { get; set; } = 2;

    [ModdedNumberOption("Revive Cooldown", 1, 50, 2, NumberSuffixes.Seconds)]
    public float ReviveCooldown { get; set; } = 20;
}