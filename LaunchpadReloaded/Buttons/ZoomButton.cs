using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using System.Collections;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

[RegisterButton]
public class ZoomButton : BaseLaunchpadButton
{
    public override string Name => "ZOOM";
    
    public override float Cooldown => OptionGroupSingleton<CaptainOptions>.Instance.ZoomCooldown;
    
    public override float EffectDuration => OptionGroupSingleton<CaptainOptions>.Instance.ZoomDuration;
    
    public override int MaxUses => 0;
    
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ZoomButton;
    public override bool TimerAffectedByPlayer => true;
    public override bool AffectedByHack => true;
    
    public static bool IsZoom { get; private set; }
    
    public override bool Enabled(RoleBehaviour? role)
    {
        return role is CaptainRole;
    }

    protected override void OnClick()
    {
        Coroutines.Start(ZoomOutCoroutine());
    }

    public override void OnEffectEnd()
    {
        Coroutines.Start(ZoomInCoroutine());
    }

    private static IEnumerator ZoomOutCoroutine()
    {
        HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
        IsZoom = true;
        var zoomDistance = OptionGroupSingleton<CaptainOptions>.Instance.ZoomDistance;
        for (var ft = Camera.main!.orthographicSize; ft < zoomDistance; ft += 0.3f)
        {
            Camera.main.orthographicSize = MeetingHud.Instance ? 3f : ft;
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
            foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;
            yield return null;
        }

        foreach (var cam in Camera.allCameras) cam.orthographicSize = zoomDistance;
        ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
    }

    private static IEnumerator ZoomInCoroutine()
    {
        for (var ft = Camera.main!.orthographicSize; ft > 3f; ft -= 0.3f)
        {
            Camera.main.orthographicSize = MeetingHud.Instance ? 3f : ft;
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
            foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;

            yield return null;
        }

        foreach (var cam in Camera.allCameras) cam.orthographicSize = 3f;
        HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
        IsZoom = false;

        ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
    }
}