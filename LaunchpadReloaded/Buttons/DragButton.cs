using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class DragButton : CustomActionButton
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.DragButton;
    
    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole || (MedicRole.DragBodies.Value && role is MedicRole);
    }

    public override bool CanUse()
    {
        return base.CanUse() && LaunchpadPlayer.LocalPlayer.deadBodyTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.inVent &&
               (!LaunchpadPlayer.LocalPlayer.Dragging || CanDrop());
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (!playerControl.GetLpPlayer().Dragging)
        {
            return;
        }
        
        // can probably be improved but whatever
        HudManager.Instance.KillButton.SetDisabled();
        HudManager.Instance.ReportButton.SetDisabled();
        HudManager.Instance.UseButton.SetDisabled();
        HudManager.Instance.SabotageButton.SetDisabled();
        HudManager.Instance.ImpostorVentButton.SetDisabled();
        HudManager.Instance.AdminButton.SetDisabled();
        HudManager.Instance.ReportButton.SetDisabled();
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
        if (!LaunchpadPlayer.LocalPlayer.deadBodyTarget)
        {
            return false;
        }

        return !PhysicsHelpers.AnythingBetween(PlayerControl.LocalPlayer.Collider, PlayerControl.LocalPlayer.Collider.bounds.center, LaunchpadPlayer.LocalPlayer.deadBodyTarget.TruePosition, Constants.ShipAndAllObjectsMask, false);
    }

    protected override void OnClick()
    {
        if (LaunchpadPlayer.LocalPlayer.Dragging)
        {
            PlayerControl.LocalPlayer.RpcStopDragging();
        }
        else
        {
            PlayerControl.LocalPlayer.RpcStartDragging(LaunchpadPlayer.LocalPlayer.deadBodyTarget.ParentId);
        }
    }



}