using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class HackButton : CustomActionButton
{
    public override string Name => "HACK";
    public override float Cooldown => (int)HackerRole.HackCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)HackerRole.HackUses.Value;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.HackButton;
    public override bool Enabled(RoleBehaviour role) => role is HackerRole;
    public override bool CanUse() => !HackingManager.Instance.AnyActiveNodes();

    protected override void OnClick()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.Role is HackerRole || player.Data.IsDead || player.Data.Disconnected)
            {
                continue;
            }

            HackingManager.RpcHackPlayer(player);
        }

        PlayerControl.LocalPlayer.RawSetColor(15);

        foreach (var node in HackingManager.Instance.nodes)
        {
            HackingManager.RpcToggleNode(ShipStatus.Instance, node.Id, true);
        }
    }
}