using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 2;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.DragButton;

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
        OverrideSprite(Sprite.LoadAsset());
    }

    public void SetDrop()
    {
        OverrideName("DROP");
        OverrideSprite(LaunchpadAssets.DropButton.LoadAsset());
    }

    public bool CanDrop()
    {
        foreach (var collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.MaxReportDistance, Constants.PlayersOnlyMask))
        {
            if (!(collider2D.tag != "DeadBody")) return true;
        }
        return false;
    }

    protected override void OnClick()
    {
        if (DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId))
        {
            DragManager.RpcStopDragging(PlayerControl.LocalPlayer);
            if (!CanDrop()) DragManager.RpcStartDragging(PlayerControl.LocalPlayer, DeadBodyTarget.ParentId);
        }
        else
        {
            DragManager.RpcStartDragging(PlayerControl.LocalPlayer, DeadBodyTarget.ParentId);
        }
    }



}