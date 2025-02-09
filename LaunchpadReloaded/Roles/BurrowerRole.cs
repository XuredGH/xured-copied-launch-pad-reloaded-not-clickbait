using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterCustomRole]
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

    public List<Vent> DiggedVents { get; } = new List<Vent>();
}
