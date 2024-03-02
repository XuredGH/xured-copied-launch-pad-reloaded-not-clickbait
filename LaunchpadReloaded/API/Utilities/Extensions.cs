using System.Linq;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.API.Utilities;

public static class Extensions
{
    public static bool ButtonTimerEnabled(this PlayerControl playerControl)
    {
        return (playerControl.moveable || playerControl.petting) && !playerControl.inVent && !playerControl.shapeshifting && (!DestroyableSingleton<HudManager>.InstanceExists || !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance && !IntroCutscene.Instance;
    }

    public static bool IsHacked(this GameData.PlayerInfo playerInfo)
    {
        return HackingManager.HackedPlayers.Contains(playerInfo.PlayerId);
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
                renderer.SetOutline(Color.red);
            }
        }
    }
    
    public static DeadBody NearestDeadBody(this PlayerControl playerControl)
    {
        return Physics2D
            .OverlapCircleAll(playerControl.transform.position, 1f, ~LayerMask.GetMask(new[] {"Ship"}))
            .Where(collider2D => collider2D.CompareTag("DeadBody"))
            .Select(collider2D => collider2D.GetComponent<DeadBody>()).FirstOrDefault(component => component && !component.Reported);
    }
}