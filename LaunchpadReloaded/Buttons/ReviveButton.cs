using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class ReviveButton : CustomActionButton
{
    public override string Name => "REVIVE";
    public override float Cooldown => 10;
    public override float EffectDuration => 0;
    public override int MaxUses => 2;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ReviveButton;
    public override bool Enabled(RoleBehaviour role) => role is MedicRole;
    public override bool CanUse()
    {
        return (RevivalManager.Instance && DeadBodyTarget is not null) && PlayerControl.LocalPlayer.CanMove &&
            !DragManager.DraggingPlayers.ContainsValue(DeadBodyTarget.ParentId);
    }

    protected override void OnClick()
    {
        RevivalManager.RpcRevive(DeadBodyTarget);
        DeadBodyTarget = null;
    }
}