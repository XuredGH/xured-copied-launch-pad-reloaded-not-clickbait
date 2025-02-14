using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

public class SheriffRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Sheriff";
    public string RoleDescription => "Take your chance by shooting a player.";
    public string RoleLongDescription => $"You can shoot players, if you shoot an {Palette.ImpostorRed.ToTextColor()}Impostor</color> you will kill him\nbut if you shoot a {Palette.CrewmateBlue.ToTextColor()}Crewmate</color>, you will die with him.";
    public Color RoleColor => LaunchpadPalette.SheriffColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.ShootButton,
        OptionsScreenshot = LaunchpadAssets.SheriffBanner,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}
