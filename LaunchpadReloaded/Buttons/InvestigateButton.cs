using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class InvestigateButton : CustomActionButton<DeadBody>
{
    public override string Name => "INVESTIGATE";
    public override float Cooldown => 1;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InvestigateButton;
    public override float Distance => PlayerControl.LocalPlayer.MaxReportDistance / 4f;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is DetectiveRole;
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
        return base.CanUse() && Target;
    }

    protected override void OnClick()
    {
        if (!Target)
        {
            return;
        }
        
        var gameObject = Object.Instantiate(LaunchpadAssets.DetectiveGame.LoadAsset(), HudManager.Instance.transform);
        var minigame = gameObject.GetComponent<JournalMinigame>();
        minigame.Open(LaunchpadPlayer.GetById(Target.ParentId));
    }
}