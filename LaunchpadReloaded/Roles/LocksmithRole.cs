using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

public class LocksmithRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Locksmith";
    public string RoleDescription => "Seal vents around the map.";
    public string RoleLongDescription => "Seal vents around the map.\nThis will prevent anyone from entering the vent.";
    public Color RoleColor => LaunchpadPalette.SealerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.ReviveButton,
        OptionsScreenshot = LaunchpadAssets.MedicBanner,
    };

    public List<SealedVentComponent> SealedVents { get; } = new List<SealedVentComponent>();

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}
