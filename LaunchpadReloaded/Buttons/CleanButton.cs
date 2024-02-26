using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CleanButton : CustomActionButton
{
    public override string Name => "";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override Sprite Sprite => SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Clean.png");

    private DeadBody _target;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole;
    }

    public override void Update(PlayerControl playerControl)
    {
        base.Update(playerControl);
        foreach (var body in Object.FindObjectsOfType<DeadBody>())
        {
            foreach (var bodyRenderer in body.bodyRenderers)
            {
                bodyRenderer.SetOutline(null);
            }
        }
        
        _target = playerControl.NearestDeadBody();
        if (_target is not null)
        {
            foreach (var renderer in _target.bodyRenderers)
            {
                renderer.SetOutline(Color.yellow);
            }
        }
    }

    protected override bool CanUse()
    {
        return base.CanUse() && _target is not null;
    }

    protected override void OnClick()
    {
        _target.Reported = true;
        _target.enabled = false;
        foreach (var bodyRenderer in _target.bodyRenderers)
        {
            bodyRenderer.enabled = false;
        }
    }
}