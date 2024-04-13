using System;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class DetectiveRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Detective";
    public ushort RoleId => (ushort)LaunchpadRoles.Detective;
    public string RoleDescription => "Investigate and find clues on murders.";
    public string RoleLongDescription => "Investigate bodies to get clues and use your instinct ability\nto see recent footsteps around you!";
    public Color RoleColor => LaunchpadPalette.DetectiveColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public override bool IsDead => false;
    public bool TargetsBodies => true;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;

    public static CustomToggleOption HideSuspects;
    public static CustomNumberOption FootstepsDuration;
    public static CustomNumberOption InstinctDuration;
    public static CustomNumberOption InstinctUses;
    public static CustomNumberOption InstinctCooldown;
    public static CustomOptionGroup Group;

    public void CreateOptions()
    {
        HideSuspects = new CustomToggleOption("Hide Suspects", false, role: typeof(DetectiveRole));
        FootstepsDuration = new CustomNumberOption("Footsteps Visibility Duration", 3, 1, 10, 1, NumberSuffixes.Seconds, role: typeof(DetectiveRole));
        InstinctDuration = new CustomNumberOption("Instinct Duration", 10, 3, 76, 3, NumberSuffixes.Seconds, role: typeof(DetectiveRole));
        InstinctUses = new CustomNumberOption("Instinct Uses", 3, 0, 10, 1, NumberSuffixes.None, zeroInfinity: true, role: typeof(DetectiveRole));
        InstinctCooldown = new CustomNumberOption("Instinct Cooldown", 15, 0, 45, 1, NumberSuffixes.Seconds, role: typeof(DetectiveRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Detective</color>",
            numberOpt: [FootstepsDuration, InstinctDuration, InstinctUses, InstinctCooldown],
            stringOpt: [],
            toggleOpt: [HideSuspects], role: typeof(DetectiveRole));
    }
}