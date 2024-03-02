using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
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
    public override Sprite Sprite => LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>("Drag.png");
    private bool _isDragging;
    private DeadBody _target;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HitmanRole;
    }

    protected override bool CanUse()
    {
        return _target is not null;
    }

    public override void Update(PlayerControl playerControl)
    {
        base.Update(playerControl);
        playerControl.UpdateBodies(new Color(125,40,40), ref _target);
    }
    
    protected override void OnClick()
    {
        _isDragging = !_isDragging;
        if (_isDragging)
        {
            OverrideName("DROP");
            OverrideSprite("Drop.png");
        }
        else
        {
            OverrideName("DRAG");
            OverrideSprite("Drag.png");
        }
    }
    
    
    
}