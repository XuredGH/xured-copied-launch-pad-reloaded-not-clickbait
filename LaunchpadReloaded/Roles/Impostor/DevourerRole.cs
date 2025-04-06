using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Impostor;

public class DevourerRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Devourer";
    public string RoleDescription => "Eat other players";
    public string RoleLongDescription => "Eat other players, making them\nindistuingashble from others.";
    public Color RoleColor => LaunchpadPalette.DevourerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.DissectButton,
        UseVanillaKillButton = true,
        OptionsScreenshot = LaunchpadAssets.SurgeonBanner,
    };
}