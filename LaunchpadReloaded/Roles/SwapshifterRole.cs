using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

public class SwapshifterRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Swapshifter";
    public string RoleDescription => "Shift and swap into other players.";
    public string RoleLongDescription => RoleDescription + "\nThis can help you frame players and disguise kills.";
    public Color RoleColor => LaunchpadPalette.SwapperColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.SwapButton,
        OptionsScreenshot = LaunchpadAssets.HackerBanner,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }
}
