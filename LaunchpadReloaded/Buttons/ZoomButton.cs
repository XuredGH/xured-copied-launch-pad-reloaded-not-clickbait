﻿using LaunchpadReloaded.API.Hud;
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

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }

    protected override void OnClick()
    {
        HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
        ZoomOut();
    }

    protected override void OnEffectEnd()
    {
        ZoomIn();
    }

    private static IEnumerator ZoomOutCoroutine()
    {
        if (Camera.main is not null)
        {
            for (var ft = Camera.main.orthographicSize; ft < 12; ft += 0.1f)
            {
                Camera.main.orthographicSize = ft;
                if (MeetingHud.Instance)
                {
                    Camera.main.orthographicSize = 3f;
                }
                foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;
                ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);

                yield return null;
            }
        }
    }

    private static IEnumerator ZoomInCoroutine()
    {
        if (Camera.main is not null)
        {
            for (var ft = Camera.main.orthographicSize; ft > 3; ft -= 0.1f)
            {
                Camera.main.orthographicSize = ft;

                if (MeetingHud.Instance)
                {
                    Camera.main.orthographicSize = 3f;
                }

                foreach (var cam in Camera.allCameras) cam.orthographicSize = Camera.main.orthographicSize;
                ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen);

                if (ft < 4f) HudManager.Instance.ShadowQuad.gameObject.SetActive(true);

                yield return null;
            }
        }
    }

    private static void ZoomOut()
    {
        Coroutines.Start(ZoomOutCoroutine());
        HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
    }

    private static void ZoomIn()
    {
        Coroutines.Start(ZoomInCoroutine());
    }
}