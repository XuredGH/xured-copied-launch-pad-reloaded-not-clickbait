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
[RegisterCustomRole((ushort)LaunchpadRoles.Jester)]
public class JesterRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICustomRole
{
    public string RoleName => "Jester";
    public string RoleDescription => "Get ejected to win";
    public string RoleLongDescription => "Convince the crew to vote you out by being suspicious.\nIf you get voted out, you win the game.";
    public Color RoleColor => LaunchpadPalette.JesterColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Neutral;
    public bool TasksCount => false;
    public bool CanUseVent => ModdedGroupSingleton<JesterOptions>.Instance.CanUseVents;
    public RoleTypes GhostRole => (RoleTypes)LaunchpadRoles.OutcastGhost;
    public override bool IsDead => false;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.JesterIcon;

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
        if (!GameManager.Instance.LogicUsables.CanUse(usable, this.Player))
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
        orCreateTask.Text = string.Concat(new string[]
            {
                LaunchpadPalette.JesterColor.ToTextColor(),
                DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FakeTasks, Il2CppSystem.Array.Empty<Il2CppSystem.Object>()),
                "</color>"
            });

    }
}