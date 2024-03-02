using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override string SpritePath => "Drag.png";
    
    private bool _dragging;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HitmanRole;
    }

    public override bool CanUse()
    {
        return DeadBodyTarget is not null;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (_dragging)
        {
            HudManager.Instance.KillButton.SetDisabled();
            HudManager.Instance.ReportButton.SetDisabled();
            HudManager.Instance.UseButton.SetDisabled();
            HudManager.Instance.SabotageButton.SetDisabled();
            HudManager.Instance.ImpostorVentButton.SetDisabled();
            HudManager.Instance.AdminButton.SetDisabled();
            HudManager.Instance.ReportButton.SetDisabled();
        }
    }

    protected override void OnClick()
    {
        _dragging = !_dragging;
        if (_dragging)
        {
            OverrideName("DROP");
            OverrideSprite("Drop.png");
            DragManager.RpcStartDragging(PlayerControl.LocalPlayer, DeadBodyTarget.ParentId);
        }
        else
        {
            OverrideName("DRAG");
            OverrideSprite("Drag.png");
            DragManager.RpcStopDragging(PlayerControl.LocalPlayer);
        }
    }
    
    
    
}