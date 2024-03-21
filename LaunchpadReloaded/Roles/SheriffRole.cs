using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;
[RegisterInIl2Cpp]
public class SheriffRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Sheriff";

    public ushort RoleId => (ushort)LaunchpadRoles.Sheriff;

    public string RoleDescription => "Take your chance by shooting a player.";

    public string RoleLongDescription => $"You can shoot players, if you shoot an {Palette.ImpostorRed.ToTextColor()}Impostor</color> you will kill him\nbut if you shoot a {Palette.CrewmateBlue.ToTextColor()}Crewmate</color>, you will die with him.";

    public Color RoleColor => LaunchpadPalette.SheriffColor;

    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public bool CanUseKill => true;

    public static CustomNumberOption ShootCooldown;
    public static CustomNumberOption Shots;
    public static CustomOptionGroup Group;

    public void CreateOptions()
    {
        ShootCooldown = new CustomNumberOption("Shot Cooldown",
            defaultValue: 45,
            0, 120,
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(SheriffRole));

        Shots = new CustomNumberOption("Shots Per Game",
            defaultValue: 3,
            1, 10,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(SheriffRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Sheriff</color>",
            numberOpt: [ShootCooldown, Shots],
            stringOpt: [],
            toggleOpt: [], role: typeof(SheriffRole));
    }
}
