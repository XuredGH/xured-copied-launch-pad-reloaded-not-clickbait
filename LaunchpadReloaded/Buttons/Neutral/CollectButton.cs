using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking.Roles;
using LaunchpadReloaded.Options.Roles.Neutral;
using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons.Neutral;

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
        return PlayerControl.LocalPlayer.GetNearestDeadBody(Distance);
    }

    public override bool IsTargetValid(DeadBody? target)
    {
        return target != null && target.enabled && !target.gameObject.TryGetComponent<ReapedBodyComponent>(out _);
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

        PlayerControl.LocalPlayer.RpcCollectSoul(Target.ParentId);

        ResetTarget();
    }
}