using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking.Roles;
using LaunchpadReloaded.Options.Roles;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class DissectButton : BaseLaunchpadButton<DeadBody>
{
    public override string Name => "Dissect";
    public override float Cooldown => OptionGroupSingleton<SurgeonOptions>.Instance.DissectCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<SurgeonOptions>.Instance.DissectUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.DissectButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is SurgeonRole;
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

        SoundManager.Instance.PlaySound(LaunchpadAssets.DissectSound.LoadAsset(), false, volume: 2);
        PlayerControl.LocalPlayer.RpcDissect(Target.ParentId);

        SetOutline(false);
        Target = null;
    }
}