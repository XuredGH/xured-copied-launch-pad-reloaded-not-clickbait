using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;


[RegisterCustomRole]
public class DetectiveRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Detective";
    public string RoleDescription => "Investigate and find clues on murders.";
    public string RoleLongDescription => "Investigate bodies to get clues and use your instinct ability\nto see recent footsteps around you!";
    public Color RoleColor => LaunchpadPalette.DetectiveColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.InvestigateButton,
        OptionsScreenshot = LaunchpadAssets.DetectiveBanner,
    };
}
