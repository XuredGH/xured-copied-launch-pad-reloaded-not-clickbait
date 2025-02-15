using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Crewmate;

public class LocksmithRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Locksmith";
    public string RoleDescription => "Seal vents around the map.";
    public string RoleLongDescription => "Seal vents around the map.\nThis will prevent anyone from entering the vent.";
    public Color RoleColor => LaunchpadPalette.SealerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.SealButton,
        OptionsScreenshot = LaunchpadAssets.MedicBanner,
    };

    public List<SealedVentComponent> SealedVents { get; } = new List<SealedVentComponent>();
}
