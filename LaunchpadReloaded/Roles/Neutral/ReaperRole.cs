using AmongUs.GameOptions;
using Il2CppSystem.Text;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Neutral;

public class ReaperRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICustomRole
{
    public string RoleName => "Reaper";
    public string RoleDescription => "Collect souls to win";
    public string RoleLongDescription => "Collect souls from dead bodies to win the game.";
    public Color RoleColor => LaunchpadPalette.ReaperColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Neutral;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        CanUseVent = false,
        GhostRole = (RoleTypes)RoleId.Get<OutcastGhostRole>(),
        Icon = LaunchpadAssets.SoulButton,
        OptionsScreenshot = LaunchpadAssets.JesterBanner,
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder)
    {
        // remove default task hint
    }

    public override bool DidWin(GameOverReason reason)
    {
        return reason == (GameOverReason)GameOverReasons.ReaperWins;
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

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (player.HasModifier<RevealedModifier>()) return true;
        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}
