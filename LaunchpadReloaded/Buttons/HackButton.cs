using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Networking;
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
    public override bool CanUse() => !HackingManager.Instance.AnyNodesActive();

    protected override void OnClick()
    {
        foreach (var node in HackingManager.Instance.nodes)
        {
            PlayerControl.LocalPlayer.RpcToggleNode(node.id, true);
        }

        PlayerControl.LocalPlayer.RawSetColor(15);
    }
}