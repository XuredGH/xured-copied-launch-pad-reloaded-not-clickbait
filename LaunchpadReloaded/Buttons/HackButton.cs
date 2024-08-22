using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class HackButton : CustomActionButton
{
    public override string Name => "HACK";
    public override float Cooldown => (int)ModdedGroupSingleton<HackerOptions>.Instance.HackCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)ModdedGroupSingleton<HackerOptions>.Instance.HackUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.HackButton;
    public override bool Enabled(RoleBehaviour role) => role is HackerRole;
    public override bool CanUse() => base.CanUse() && !HackingManager.Instance.AnyPlayerHacked();

    protected override void OnClick()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.Role.IsImpostor || player.Data.IsDead || player.Data.Disconnected)
            {
                continue;
            }

            PlayerControl.LocalPlayer.RpcHackPlayer(player);
        }

        PlayerControl.LocalPlayer.RawSetColor(15);
    }
}