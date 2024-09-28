using AmongUs.GameOptions;
using Il2CppInterop.Runtime.Attributes;
using Il2CppSystem.Text;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class JesterRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICustomRole
{
    public string RoleName => "Jester";
    public string RoleDescription => "Get ejected to win";
    public string RoleLongDescription => "Convince the crew to vote you out by being suspicious.\nIf you get voted out, you win the game.";
    public Color RoleColor => LaunchpadPalette.JesterColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Neutral;

    public CustomRoleConfiguration Configuration => new()
    {
        TasksCountForProgress = false,
        CanUseVent = OptionGroupSingleton<JesterOptions>.Instance.CanUseVents,
        GhostRole = (RoleTypes)RoleId.Get<OutcastGhostRole>(),
        Icon = LaunchpadAssets.JesterIcon,
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override bool DidWin(GameOverReason reason)
    {
        return reason == (GameOverReason)GameOverReasons.JesterWins;
    }

    public string GetCustomEjectionMessage(NetworkedPlayerInfo exiled)
    {
        return $"You've been fooled! {exiled.PlayerName} was The Jester.";
    }

    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }

        var console = usable.TryCast<Console>();
        return !(console != null) || console.AllowImpostor;
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        if (playerControl != PlayerControl.LocalPlayer) return;
        var orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = string.Concat([
            LaunchpadPalette.JesterColor.ToTextColor(),
                DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FakeTasks, Il2CppSystem.Array.Empty<Il2CppSystem.Object>()),
                "</color>"
        ]);

    }
}