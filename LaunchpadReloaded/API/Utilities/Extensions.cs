using System.Linq;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.API.Utilities;

public static class Extensions
{
    private static readonly ContactFilter2D Filter = ContactFilter2D.CreateLegacyFilter(Constants.NotShipMask, float.MinValue, float.MaxValue);
    
    public static bool ButtonTimerEnabled(this PlayerControl playerControl)
    {
        return (playerControl.moveable || playerControl.petting) && !playerControl.inVent && !playerControl.shapeshifting && (!DestroyableSingleton<HudManager>.InstanceExists || !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance && !IntroCutscene.Instance;
    }

    public static bool IsHacked(this GameData.PlayerInfo playerInfo)
    {
        return HackingManager.HackedPlayers.Contains(playerInfo.PlayerId);
    }

    public static void HideBody(this DeadBody body)
    {
        body.Reported = true;
        body.enabled = false;
        foreach (var spriteRenderer in body.bodyRenderers)
        {
            spriteRenderer.enabled = false;
        }
    }

    public static void ShowBody(this DeadBody body, bool reported)
    {
        body.Reported = reported;
        body.enabled = true;
        foreach (var spriteRenderer in body.bodyRenderers)
        {
            spriteRenderer.enabled = true;
        }
    }
    
    public static void UpdateBodies(this PlayerControl playerControl, Color outlineColor, ref DeadBody target)
    {
        
        foreach (var body in Object.FindObjectsOfType<DeadBody>())
        {
            foreach (var bodyRenderer in body.bodyRenderers)
            {
                bodyRenderer.SetOutline(null);
            }
        }
        
        target = playerControl.NearestDeadBody();
        if (target is not null)
        {
            foreach (var renderer in target.bodyRenderers)
            {
                renderer.SetOutline(outlineColor);
            }
        }
    }
    
    public static DeadBody NearestDeadBody(this PlayerControl playerControl)
    {
        var results = new Il2CppSystem.Collections.Generic.List<Collider2D>();
        Physics2D.OverlapCircle(playerControl.GetTruePosition(), playerControl.MaxReportDistance/4f, Filter, results);
        return results.ToArray()
            .Where(collider2D => collider2D.CompareTag("DeadBody"))
            .Select(collider2D => collider2D.GetComponent<DeadBody>())
            .FirstOrDefault(component => component && !component.Reported);
    }
}