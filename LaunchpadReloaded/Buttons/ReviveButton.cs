using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class ReviveButton : CustomActionButton
{
    public override string Name => "REVIVE";
    
    public override float Cooldown => ModdedGroupSingleton<MedicOptions>.Instance.ReviveCooldown;
    
    public override float EffectDuration => 0;
    
    public override int MaxUses => (int)ModdedGroupSingleton<MedicOptions>.Instance.MaxRevives;
    
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ReviveButton;
    
    public override bool Enabled(RoleBehaviour role) => role is MedicRole;

    public override bool CanUse()
    {
        return base.CanUse() && CanRevive() && LaunchpadPlayer.LocalPlayer.deadBodyTarget && 
               !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove && !LaunchpadPlayer.LocalPlayer.Dragging;
    }

    public bool CanRevive()
    {
        if (!ModdedGroupSingleton<MedicOptions>.Instance.OnlyAllowInMedbay)
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
        PlayerControl.LocalPlayer.RpcRevive(LaunchpadPlayer.LocalPlayer.deadBodyTarget.ParentId);
    }
}