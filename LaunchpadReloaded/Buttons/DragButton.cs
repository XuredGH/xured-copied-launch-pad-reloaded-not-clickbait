using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
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
        return role is JanitorRole || (MedicRole.DragBodies.Value && role is MedicRole);
    }

    public override bool CanUse()
    {
        return DeadBodyTarget && DragManager.Instance && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.inVent;
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (DragManager.Instance && DragManager.Instance.IsDragging(playerControl.PlayerId))
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
        if (DeadBodyTarget || DragManager.Instance is null)
        {
            return false;
        }

        return !PhysicsHelpers.AnythingBetween(PlayerControl.LocalPlayer.Collider, PlayerControl.LocalPlayer.Collider.bounds.center, DeadBodyTarget.TruePosition, Constants.ShipAndAllObjectsMask, false);
    }

    protected override void OnClick()
    {
        if (DragManager.Instance.IsDragging(PlayerControl.LocalPlayer.PlayerId))
        {
            DragManager.RpcStopDragging(PlayerControl.LocalPlayer);
        }
        else
        {
            DragManager.RpcStartDragging(PlayerControl.LocalPlayer, DeadBodyTarget.ParentId);
        }
    }



}