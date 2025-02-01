using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using PowerTools;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class DissectButton : BaseLaunchpadButton<DeadBody>
{
    public override string Name => "Dissect";
    public override float Cooldown => 1;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
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
            renderer.SetOutline(active ? PlayerControl.LocalPlayer.Data.Role.NameColor : null);
        }
    }

    protected override void OnClick()
    {
        if (Target == null)
        {
            return;
        }
        var bone = new GameObject("Bone").AddComponent<SpriteRenderer>();
        bone.sprite = LaunchpadAssets.Bone.LoadAsset();
        bone.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, 0);
        bone.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        bone.gameObject.layer = Target.gameObject.layer;
        Target.gameObject.SetActive(false);
    }
}