using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles.Neutral;
using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using Helpers = MiraAPI.Utilities.Helpers;

namespace LaunchpadReloaded.Buttons;

public class CollectButton : BaseLaunchpadButton<DeadBody>
{
    public override string Name => "Collect Soul";
    public override float Cooldown => OptionGroupSingleton<ReaperOptions>.Instance.CollectCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.SoulButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is ReaperRole;
    }

    public override DeadBody? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestObjectOfType<DeadBody>(Distance, Helpers.CreateFilter(Constants.NotShipMask), "DeadBody");
    }

    public override bool IsTargetValid(DeadBody? target)
    {
        return target != null && target.enabled;
    }

    public override void SetOutline(bool active)
    {
        if (Target == null || Target.Reported)
        {
            return;
        }

        foreach (var renderer in Target.bodyRenderers)
        {
            renderer.UpdateOutline(active ? PlayerControl.LocalPlayer.Data.Role.NameColor : null);
        }
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        ResetTarget();
    }
}