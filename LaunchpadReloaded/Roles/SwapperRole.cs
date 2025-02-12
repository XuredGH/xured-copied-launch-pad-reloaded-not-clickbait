using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;


[RegisterCustomRole]
public class SwapperRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole, ILaunchpadRole
{
    public string RoleName => "Swapper";
    public string RoleDescription => "You can swap locations with any player on the map.";
    public string RoleLongDescription => RoleDescription + "\nThis can help you frame other players.";
    public Color RoleColor => LaunchpadPalette.SwapperColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.SwapButton,
        OptionsScreenshot = LaunchpadAssets.HackerBanner,
    };

    public bool CanSeeRoleTag()
    {
        var baseVisibility = OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
        return baseVisibility || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }
}
