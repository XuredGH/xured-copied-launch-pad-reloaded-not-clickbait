using System.Collections;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Roles;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class ZoomButton : CustomActionButton
{
    public override string Name => "ZOOM";
    public override float Cooldown => 10;
    public override float EffectDuration => 5;
    public override int MaxUses => 0;
    public override string SpritePath => "Zoom.png";

    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }
    
    protected override void OnClick()
    {
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
        HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
    }
}