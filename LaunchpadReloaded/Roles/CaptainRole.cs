using System;
using System.Collections;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class CaptainRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Captain";
    public string RoleDescription => "Protect the crew with your abilities";
    public string RoleLongDescription => "Use your zoom ability to keep an eye on the crew and call a meeting from any location!";
    public Color RoleColor => Color.gray;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public CustomButton[] CustomButtons => [ZoomButton, MeetingButton];

    public override bool IsDead => false;
    
    public static readonly CustomButton ZoomButton = new (ZoomOutCoroutine, ZoomInCoroutine, "ZOOM", 5, 5,
        "LaunchpadReloaded.Resources.binoculars.png");
    public static readonly CustomButton MeetingButton = new (() => PlayerControl.LocalPlayer.CmdReportDeadBody(null), "CALL", 5,
        "LaunchpadReloaded.Resources.report.png", maxUses:3);

    public CaptainRole()
    {
        Debug.LogError("CAPTAIN INIT");
    }

    public CaptainRole(IntPtr ptr) : base(ptr)
    {
    }

    private static IEnumerator ZoomOut()
    {
        for (var ft = 3f; ft <= 13; ft += 0.5f)
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
        for (var ft = 13f; ft >= 3; ft -= 0.5f)
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