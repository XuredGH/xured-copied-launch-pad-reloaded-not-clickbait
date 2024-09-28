﻿using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class ReviveButton : CustomActionButton<DeadBody>
{
    public override string Name => "REVIVE";
    
    public override float Cooldown => OptionGroupSingleton<MedicOptions>.Instance.ReviveCooldown;
    
    public override float EffectDuration => 0;
    
    public override int MaxUses => (int)OptionGroupSingleton<MedicOptions>.Instance.MaxRevives;
    
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ReviveButton;
    
    public override float Distance => PlayerControl.LocalPlayer.MaxReportDistance / 4f;
    
    public override bool Enabled(RoleBehaviour? role) => role is MedicRole;

    public override void SetOutline(bool active)
    {
        if (Target == null)
        {
            return;
        }
        
        foreach (var renderer in Target.bodyRenderers)
        {
            renderer.SetOutline(active ? LaunchpadPalette.MedicColor : null);
        }
    }
    
    public override DeadBody? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestObjectOfType<DeadBody>(Distance, Helpers.CreateFilter(Constants.NotShipMask), "DeadBody");
    }

    public override bool IsTargetValid(DeadBody? target)
    {
        return target != null && !target.Reported;
    }
    
    public override bool CanUse()
    {
        return base.CanUse() && CanRevive() && Target && 
               !PlayerControl.LocalPlayer.Data.IsDead &&
               PlayerControl.LocalPlayer.CanMove &&
               !PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>();
    }

    private static bool CanRevive()
    {
        if (!OptionGroupSingleton<MedicOptions>.Instance.OnlyAllowInMedbay)
        {
            return true;
        }

        try
        {
            return ShipStatus.Instance.FastRooms[SystemTypes.MedBay].roomArea
            .OverlapPoint(PlayerControl.LocalPlayer.GetTruePosition());
        }
        catch
        {
            try
            {
                return ShipStatus.Instance.FastRooms[SystemTypes.Laboratory].roomArea
                .OverlapPoint(PlayerControl.LocalPlayer.GetTruePosition());
            }
            catch
            {
                return ShipStatus.Instance.FastRooms[SystemTypes.Medical].roomArea
                .OverlapPoint(PlayerControl.LocalPlayer.GetTruePosition());
            }
        }
    }
    
    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }
        
        PlayerControl.LocalPlayer.RpcRevive(Target.ParentId);
    }
}