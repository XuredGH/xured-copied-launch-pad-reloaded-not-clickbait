using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using System.Collections;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class ZoomButton : CustomActionButton
{
    public override string Name => "ZOOM";
    public override float Cooldown => 10;
    public override float EffectDuration => 5;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ZoomButton;
    public static bool IsZoom = false;
    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    public override bool CanUse() => !HackingManager.Instance.AnyActiveNodes();

    protected override void OnClick()
    {
        ZoomOut();
    }

    protected override void OnEffectEnd()
    {
        ZoomIn();
    }

    public static IEnumerator ZoomOutCoroutine()
    {
        HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
        IsZoom = true;
        for (var ft = Camera.main.orthographicSize; ft < 9; ft += 0.1f)
        {
            Camera.main.orthographicSize = MeetingHud.Instance ? 3f : ft;
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
            foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;
            yield return null;
        }
    }

    public static IEnumerator ZoomInCoroutine()
    {
        for (var ft = Camera.main.orthographicSize; ft > 3; ft -= 0.1f)
        {
            Camera.main.orthographicSize = MeetingHud.Instance ? 3f : ft;
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);
            foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;

            yield return null;
        }

        HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
        IsZoom = false;
    }

    private static void ZoomOut()
    {
        Coroutines.Start(ZoomOutCoroutine());
    }

    private static void ZoomIn()
    {
        Coroutines.Start(ZoomInCoroutine());
    }
}