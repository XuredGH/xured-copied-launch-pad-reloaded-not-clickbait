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
    private DeadBody _target;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HitmanRole;
    }

    public override bool CanUse()
    {
        return _target is not null;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        playerControl.UpdateBodies(new Color(125,40,40), ref _target);
    }
    
    protected override void OnClick()
    {
        _dragging = !_dragging;
        if (_dragging)
        {
            OverrideName("DROP");
            OverrideSprite("Drop.png");
            DragManager.RpcStartDragging(PlayerControl.LocalPlayer, _target.ParentId);
        }
        else
        {
            OverrideName("DRAG");
            OverrideSprite("Drag.png");
            DragManager.RpcStopDragging(PlayerControl.LocalPlayer);
        }
    }
    
    
    
}