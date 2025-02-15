using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Crewmate;

public class CoronerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Coroner";
    public string RoleDescription => "Freeze bodies to prevent them from disappearing.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => LaunchpadPalette.CoronerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.FreezeButton,
        OptionsScreenshot = LaunchpadAssets.CaptainBanner,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}