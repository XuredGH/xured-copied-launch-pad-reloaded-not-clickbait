﻿using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Impostor;

public class BurrowerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Burrower";
    public string RoleDescription => "Create vents around the map.";
    public string RoleLongDescription => "Move around the map easier\nBy digging new vents.";
    public Color RoleColor => LaunchpadPalette.BurrowerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.DigVentButton,
        OptionsScreenshot = LaunchpadAssets.HackerBanner,
    };

    [HideFromIl2Cpp]
    public List<Vent> DugVents { get; } = [];

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (PlayerControl.LocalPlayer.HasModifier<HackedModifier>()) return false;
        if (player.HasModifier<RevealedModifier>()) return true;

        return PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }
}
