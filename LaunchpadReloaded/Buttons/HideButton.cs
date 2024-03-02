using System.Linq;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class HideButton : CustomActionButton
{
    public override string Name => "HIDE";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 3;
    public override string SpritePath => "Clean.png";

    private Vent VentTarget => HudManager.Instance.ImpostorVentButton.currentTarget;
    
    public override bool CanUse()
    {
        return DeadBodyTarget is not null && 
               DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId) &&
               VentTarget;
    }

    protected override void OnClick()
    {
        DeadBodyManager.RpcStopDragging(PlayerControl.LocalPlayer);
        DeadBodyManager.RpcHideBodyInVent(ShipStatus.Instance, DeadBodyTarget.ParentId, VentTarget.Id);
    }

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HitmanRole;
    }
}