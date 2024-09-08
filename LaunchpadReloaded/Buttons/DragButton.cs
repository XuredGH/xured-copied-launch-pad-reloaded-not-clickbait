using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class DragButton : CustomActionButton<DeadBody>
{
    public override string Name => "DRAG";
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.DragButton;
    public override float Distance => PlayerControl.LocalPlayer.MaxReportDistance / 4f;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is JanitorRole || (OptionGroupSingleton<MedicOptions>.Instance.DragBodies && role is MedicRole);
    }

    public override DeadBody? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestObjectOfType<DeadBody>(Distance, "DeadBody");
    }

    public override bool IsTargetValid(DeadBody target)
    {
        return target && !target.Reported;
    }
    
    public override void SetOutline(bool active)
    {
        if (!Target)
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
        return base.CanUse() && Target && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.inVent &&
               (!PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>()|| CanDrop());
    }

    protected override void FixedUpdate(PlayerControl playerControl)
    {
        if (!playerControl.HasModifier<DragBodyModifier>())
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

    private bool CanDrop()
    {
        if (Target == null)
        {
            return false;
        }

        return !PhysicsHelpers.AnythingBetween(PlayerControl.LocalPlayer.Collider, PlayerControl.LocalPlayer.Collider.bounds.center, Target.TruePosition, Constants.ShipAndAllObjectsMask, false);
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }
        
        if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
        {
            PlayerControl.LocalPlayer.RpcStopDragging();
        }
        else
        {
            PlayerControl.LocalPlayer.RpcStartDragging(Target.ParentId);
        }
    }



}