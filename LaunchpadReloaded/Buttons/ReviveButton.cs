using LaunchpadReloaded.API.Hud;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class ReviveButton : CustomActionButton
{
    public override string Name => "REVIVE";
    public override float Cooldown => 10;
    public override float EffectDuration => 0;
    public override int MaxUses => 5;
    public override Sprite Sprite => LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>("Revive.png");

    public override bool Enabled(RoleBehaviour role)
    {
        return false;
    }

    protected override void OnClick()
    {
    }
}