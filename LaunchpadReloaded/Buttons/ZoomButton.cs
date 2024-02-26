using System.Collections;
using AmongUs.GameOptions;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Buttons;

public class ZoomButton : CustomActionButton
{
    public override string Name => "";
    public override float Cooldown => 10;
    public override float EffectDuration => 5;
    public override int MaxUses => 0;
    public override Sprite Sprite => SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Zoom.png");
    
    public override bool Enabled(RoleBehaviour role)
    {
        return role is CaptainRole;
    }
    
    protected override void OnClick()
    {
        ZoomOutCoroutine();
    }

    protected override void OnEffectEnd()
    {
        base.OnEffectEnd();
        ZoomInCoroutine();
    }

    private static IEnumerator ZoomOut()
    {
        for (var ft = 3f; ft <= 12; ft += 0.25f)
        {
            if (Camera.main is not null)
            {
                Camera.main.orthographicSize = ft;
                if (MeetingHud.Instance)
                    Camera.main.orthographicSize = 3f;
            }

            yield return null;
        }
    }

    private static IEnumerator ZoomIn()
    {
        for (var ft = 12f; ft >= 3; ft -= 0.25f)
        {
            if (Camera.main is not null)
            {
                Camera.main.orthographicSize = ft;
                if (MeetingHud.Instance)
                    Camera.main.orthographicSize = 3f;
            }

            yield return null;
        }
    }

    private static void ZoomOutCoroutine()
    {
        Coroutines.Start(ZoomOut());
        HudManager.Instance.ShadowQuad.gameObject.SetActive(false);
    }

    private static void ZoomInCoroutine()
    {
        Coroutines.Start(ZoomIn());
        HudManager.Instance.ShadowQuad.gameObject.SetActive(true);
    }
}