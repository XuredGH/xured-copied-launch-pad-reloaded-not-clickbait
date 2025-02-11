using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;


[RegisterCustomRole]
public class SurgeonRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole, ILaunchpadRole
{
    public string RoleName => "Surgeon";
    public string RoleDescription => "Poison other players and dissect bodies";
    public string RoleLongDescription => "You can poison players resulting in their death\nand you can dissect bodies to make them unreportable.";
    public Color RoleColor => LaunchpadPalette.SurgeonColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.DissectButton,
        //        UseVanillaKillButton = OptionGroupSingleton<SurgeonOptions>.Instance.StandardKill, NEEDS FIX
        OptionsScreenshot = LaunchpadAssets.SurgeonBanner,
    };

    public bool CanSeeRoleTag()
    {
        var baseVisibility = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
        return baseVisibility || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }
}