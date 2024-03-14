using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;
public class HackButton : CustomActionButton
{
    public override string Name => "HACK";
    public override float Cooldown => (int)LaunchpadGameOptions.Instance.HackCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)LaunchpadGameOptions.Instance.HackUses.Value;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.HackButton;
    public override bool Enabled(RoleBehaviour role) => role is HackerRole;
    public override bool CanUse() => !HackingManager.Instance.AnyActiveNodes() && !TutorialManager.InstanceExists;
    
    protected override void OnClick()
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.Role is HackerRole)
            {
                continue;
            }

            HackingManager.RpcHackPlayer(player);
        }

        PlayerControl.LocalPlayer.RawSetColor(15);
        HackingManager.RpcToggleNode(ShipStatus.Instance, HackingManager.Instance.Nodes.Random().Id, true);
    }
}