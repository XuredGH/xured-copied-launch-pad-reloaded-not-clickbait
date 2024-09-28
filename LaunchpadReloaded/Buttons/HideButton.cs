using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class HideButton : BaseLaunchpadButton<DeadBody>
{
    public override string Name => "HIDE";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 3;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.HideButton;
    public override float Distance => PlayerControl.LocalPlayer.MaxReportDistance / 4f;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => true;

    private static Vent VentTarget => HudManager.Instance.ImpostorVentButton.currentTarget;

    public override DeadBody? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestObjectOfType<DeadBody>(Distance, Helpers.CreateFilter(Constants.NotShipMask), "DeadBody");
    }

    public override bool IsTargetValid(DeadBody? target)
    {
        return target != null && !target.Reported;
    }

    public override void SetOutline(bool active)
    {
        if (Target == null)
        {
            return;
        }
        
        foreach (var renderer in Target.bodyRenderers)
        {
            renderer.SetOutline(active ? PlayerControl.LocalPlayer.Data.Role.NameColor : null);
        }
    }

    public override bool CanUse()
    {
        return base.CanUse() &&
               Target &&
               PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>() &&
               VentTarget && !VentTarget.gameObject.GetComponent<VentBodyComponent>();
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }
        
        PlayerControl.LocalPlayer.RpcStopDragging();
        PlayerControl.LocalPlayer.RpcHideBodyInVent(Target.ParentId, VentTarget.Id);
    }

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is JanitorRole;
    }
}