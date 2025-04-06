using AmongUs.GameOptions;
using Il2CppSystem.Text;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.GameOver;
using LaunchpadReloaded.Options.Roles.Neutral;
using LaunchpadReloaded.Roles.Afterlife.Outcast;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Neutral;

public class ChickenRole(System.IntPtr ptr) : RoleBehaviour(ptr), IOutcastRole
{
    public string RoleName => "Chicken";
    public string RoleDescription => "Peck everyone to win";
    public string RoleLongDescription => "";
    public Color RoleColor => LaunchpadPalette.ChickenColor;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        CanUseVent = OptionGroupSingleton<ChickenOptions>.Instance.CanUseVents,
        GhostRole = (RoleTypes)RoleId.Get<OutcastGhostRole>(),
        Icon = LaunchpadAssets.JesterIcon,
        OptionsScreenshot = LaunchpadAssets.JesterBanner,
        UseVanillaKillButton = true,
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder)
    {
        // remove default task hint
    }

    public override bool DidWin(GameOverReason reason)
    {
        return reason == CustomGameOver.GameOverReason<JesterGameOver>();
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
        if (playerControl != PlayerControl.LocalPlayer)
        {
            return;
        }

        var orCreateTask = PlayerTask.GetOrCreateTask<ImportantTextTask>(playerControl);
        orCreateTask.Text = string.Concat([
            LaunchpadPalette.ChickenColor.ToTextColor(),
                DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FakeTasks, Il2CppSystem.Array.Empty<Il2CppSystem.Object>()),
                "</color>"
        ]);
    }
}
