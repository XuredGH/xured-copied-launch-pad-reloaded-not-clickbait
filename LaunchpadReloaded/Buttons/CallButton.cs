using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class CallButton : CustomActionButton
{
    public override string Name => "CALL";
    public override float Cooldown => CaptainRole.CaptainMeetingCooldown.Value;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)CaptainRole.CaptainMeetingCount.Value;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.CallButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    public override bool CanUse() => !ZoomButton.IsZoom && !HackingManager.Instance.AnyPlayerHacked();

    protected override void OnClick()
    {
        var bt = ShipStatus.Instance.EmergencyButton;
        
        PlayerControl.LocalPlayer.NetTransform.Halt();
        var minigame = Object.Instantiate(bt.MinigamePrefab, Camera.main.transform, false);
        
        var taskAdderGame = minigame as TaskAdderGame;
        if (taskAdderGame != null)
        {
            taskAdderGame.SafePositionWorld = bt.SafePositionLocal + (Vector2)bt.transform.position;
        }

        minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
        minigame.Begin(null);
    }
}