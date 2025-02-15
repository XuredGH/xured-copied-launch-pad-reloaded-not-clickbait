﻿using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Crewmate;

public class MedicRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Medic";
    public string RoleDescription => "Help the crewmates by reviving dead players.";
    public string RoleLongDescription => "Use your revive ability to bring dead bodies\nback to life!";
    public Color RoleColor => LaunchpadPalette.MedicColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.ReviveButton,
        OptionsScreenshot = LaunchpadAssets.MedicBanner,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (PlayerControl.LocalPlayer.HasModifier<HackedModifier>()) return false;
        if (player.HasModifier<RevealedModifier>()) return true;

        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}
