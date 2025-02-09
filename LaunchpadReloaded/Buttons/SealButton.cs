using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking.Roles;
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
public class SealButton : BaseLaunchpadButton<Vent>
{
    public override string Name => "Seal";
    public override float Cooldown => OptionGroupSingleton<SealerOptions>.Instance.SealVentCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<SealerOptions>.Instance.SealVentUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.DissectButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => false;
    public override float Distance => 1f;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is SealerRole;
    }

    public override Vent? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetNearestObjectOfType<Vent>(Distance, Helpers.CreateFilter(Constants.NotShipMask));
    }

    public override bool IsTargetValid(Vent? target)
    {
        return target != null && target.enabled && target.gameObject.GetComponent<SealedVentComponent>() == null;
    }

    public override void SetOutline(bool active)
    {
        if (Target == null)
        {
            return;
        }

        Target.myRend.SetOutline(active ? PlayerControl.LocalPlayer.Data.Role.NameColor : null);
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }

        PlayerControl.LocalPlayer.RpcSealVent(Target.Id);
        Target = null;
    }
}