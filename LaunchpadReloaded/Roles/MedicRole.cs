using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class MedicRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Medic";
    public ushort RoleId => (ushort)LaunchpadRoles.Medic;
    public string RoleDescription => "Help the crewmates by reviving dead players.";
    public string RoleLongDescription => "Use your revive ability to bring dead bodies\nback to life!";
    public Color RoleColor => LaunchpadPalette.MedicColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public override bool IsDead => false;
    public bool TargetsBodies => true;
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ReviveButton;
    public static CustomToggleOption OnlyAllowInMedbay;
    public static CustomNumberOption MaxRevives;
    public static CustomNumberOption ReviveCooldown;
    public static CustomOptionGroup Group;

    public void CreateOptions()
    {
        MaxRevives = new CustomNumberOption("Max Revives",
            defaultValue: 2,
            1, 9,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(MedicRole));

        ReviveCooldown = new CustomNumberOption("Revive Cooldown",
            defaultValue: 20,
            1, 50,
            increment: 2,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(MedicRole));

        OnlyAllowInMedbay = new CustomToggleOption("Only Allow Reviving in MedBay/Laboratory",
            defaultValue: false,
            role: typeof(MedicRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Janitor</color>",
            numberOpt: [MaxRevives, ReviveCooldown],
            stringOpt: [],
            toggleOpt: [OnlyAllowInMedbay],
            role: typeof(JanitorRole));
    }
}