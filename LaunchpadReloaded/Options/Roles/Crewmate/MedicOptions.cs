using LaunchpadReloaded.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;

namespace LaunchpadReloaded.Options.Roles.Crewmate;

public class MedicOptions : AbstractOptionGroup
{
    public override string GroupName => "Medic";

    public override Type AdvancedRole => typeof(MedicRole);

    [ModdedToggleOption("Only Allow Reviving in MedBay/Laboratory")]
    public bool OnlyAllowInMedbay { get; set; } = false;

    [ModdedToggleOption("Can Drag Bodies")]
    public bool DragBodies { get; set; } = false;

    [ModdedNumberOption("Max Revives", 1, 9)]
    public float MaxRevives { get; set; } = 2;

    [ModdedNumberOption("Revive Cooldown", 1, 50, 2, MiraNumberSuffixes.Seconds)]
    public float ReviveCooldown { get; set; } = 20;
}