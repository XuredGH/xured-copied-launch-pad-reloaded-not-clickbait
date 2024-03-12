using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override Sprite Sprite => LaunchpadAssets.DragButton;

    public static DragButton Instance;

    public DragButton()
    {
        Instance = this;
    }
    
    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole;
    }

    public override bool CanUse()
    {
        return DeadBodyTarget is not null && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.inVent;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (DragManager.IsDragging(playerControl.PlayerId))
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

    public void SetDrag()
    {
        OverrideName("DRAG");
        OverrideSprite(LaunchpadAssets.DragButton);
    }

    public void SetDrop()
    {
        OverrideName("DROP");
        OverrideSprite(LaunchpadAssets.DropButton);
    }

    protected override void OnClick()
    {
        if (DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId))
        {
            DragManager.RpcStopDragging(PlayerControl.LocalPlayer);
        }
        else
        {
            DragManager.RpcStartDragging(PlayerControl.LocalPlayer, DeadBodyTarget.ParentId);
        }
    }
    
    
    
}