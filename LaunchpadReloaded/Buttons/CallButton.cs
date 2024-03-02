using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Roles;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class CallButton : CustomActionButton
{
    public override string Name => "CALL";
    public override float Cooldown => 5;
    public override float EffectDuration => 0;
    public override int MaxUses => 3;
    public override Sprite Sprite => LaunchpadReloadedPlugin.Bundle.LoadAsset<Sprite>("CallMeeting.png");

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    protected override void OnClick()
    {
        PlayerControl.LocalPlayer.CmdReportDeadBody(null);
    }
}