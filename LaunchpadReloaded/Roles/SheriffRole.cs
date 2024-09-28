using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class SheriffRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Sheriff";
    public string RoleDescription => "Take your chance by shooting a player.";
    public string RoleLongDescription => $"You can shoot players, if you shoot an {Palette.ImpostorRed.ToTextColor()}Impostor</color> you will kill him\nbut if you shoot a {Palette.CrewmateBlue.ToTextColor()}Crewmate</color>, you will die with him.";
    public Color RoleColor => LaunchpadPalette.SheriffColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new()
    {
        Icon = LaunchpadAssets.ShootButton,
    };
}
