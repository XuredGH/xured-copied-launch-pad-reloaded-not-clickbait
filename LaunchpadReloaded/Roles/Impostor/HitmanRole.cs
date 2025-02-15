using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Impostor;

public class HitmanRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Hitman";
    public string RoleDescription => "Slow down time and kill the Crewmates.";
    public string RoleLongDescription => "Slow down time and kill the Crewmates.\nYou can kill multiple players at once.";
    public Color RoleColor => LaunchpadPalette.HitmanColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public HitmanManagerComponent Manager;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.DragButton,
        OptionsScreenshot = LaunchpadAssets.JanitorBanner,
        UseVanillaKillButton = false,
    };
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (Manager == null)
        {
            Manager = Player.gameObject.AddComponent<HitmanManagerComponent>();
        }
        Manager.deadEyeCooldown = OptionGroupSingleton<HitmanOptions>.Instance.DeadlockCooldown;
        Manager.deadEyeLimit = OptionGroupSingleton<HitmanOptions>.Instance.DeadlockDuration;
        Manager.Player = playerControl;
        Manager.Initialize();
    }
    public override void Deinitialize(PlayerControl targetPlayer)
    {
        Destroy(Manager);
    }

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead || PlayerControl.LocalPlayer.Data.Role.IsImpostor;
    }
}