using System.Linq;
using System.Reflection;
using AmongUs.GameOptions;
using Il2CppSystem.Collections.Generic;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class Extensions
{
    private static readonly ContactFilter2D Filter = ContactFilter2D.CreateLegacyFilter(Constants.NotShipMask, float.MinValue, float.MaxValue);
    
    public static void SetGradientData(this GameObject gameObject, byte playerId)
    {
        var data = gameObject.GetComponent<PlayerGradientData>();
        if (!data)
        {
            data = gameObject.AddComponent<PlayerGradientData>();
        }
        data.playerId = playerId;
    }

    public static bool ButtonTimerEnabled(this PlayerControl playerControl)
    {
        return (playerControl.moveable || playerControl.petting) && !playerControl.inVent && !playerControl.shapeshifting && (!DestroyableSingleton<HudManager>.InstanceExists || !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance && !IntroCutscene.Instance;
    }

    public static bool IsHacked(this GameData.PlayerInfo playerInfo)
    {
        if (!HackingManager.Instance)
        {
            return false;
        }

        return HackingManager.Instance.hackedPlayers.Contains(playerInfo.PlayerId) || (playerInfo.Role is HackerRole && HackingManager.Instance.AnyPlayerHacked());
    }

    public static bool IsRevived(this PlayerControl player)
    {
        return RevivalManager.Instance is not null && RevivalManager.Instance.revivedPlayers.Contains(player.PlayerId);
    }

    public static void Revive(this DeadBody body)
    {
        var player = PlayerControl.AllPlayerControls.ToArray().ToList().Find(player => player.PlayerId == body.ParentId);
        player.NetTransform.SnapTo(body.transform.position);
        player.Revive();

        player.RemainingEmergencies = GameManager.Instance.LogicOptions.GetNumEmergencyMeetings();
        RoleManager.Instance.SetRole(player, RoleTypes.Crewmate);
        player.Data.Role.SpawnTaskHeader(player);
        player.MyPhysics.SetBodyType(player.BodyType);

        if (player.AmOwner)
        {
            HudManager.Instance.MapButton.gameObject.SetActive(true);
            HudManager.Instance.ReportButton.gameObject.SetActive(true);
            HudManager.Instance.UseButton.gameObject.SetActive(true);
            player.myTasks.RemoveAt(0);
        }

        body.gameObject.Destroy();
        RevivalManager.Instance.revivedPlayers.Add(player.PlayerId);
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
    public static bool IsOverride(this MethodInfo methodInfo)
    {
        return methodInfo.GetBaseDefinition() != methodInfo;
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

        if (playerControl.Data.Role is not ICustomRole { TargetsBodies: true })
        {
            return;
        }

        target = playerControl.NearestDeadBody();
        if (target)
        {
            foreach (var renderer in target.bodyRenderers)
            {
                renderer.SetOutline(outlineColor);
            }
        }

    }

    public static DeadBody NearestDeadBody(this PlayerControl playerControl)
    {
        var results = new List<Collider2D>();
        Physics2D.OverlapCircle(playerControl.GetTruePosition(), playerControl.MaxReportDistance / 4f, Filter, results);
        return results.ToArray()
            .Where(collider2D => collider2D.CompareTag("DeadBody"))
            .Select(collider2D => collider2D.GetComponent<DeadBody>())
            .FirstOrDefault(component => component && !component.Reported);
    }

    public static PlayerControl GetClosestPlayer(this PlayerControl playerControl, bool includeImpostors, float distance)
    {
        PlayerControl result = null;
        if (!ShipStatus.Instance)
        {
            return null;
        }

        var truePosition = playerControl.GetTruePosition();

        foreach (var playerInfo in GameData.Instance.AllPlayers)
        {
            if (playerInfo.Disconnected || playerInfo.PlayerId == playerControl.PlayerId ||
                playerInfo.IsDead || !includeImpostors && playerInfo.Role.IsImpostor)
            {
                continue;
            }

            var @object = playerInfo.Object;
            if (!@object)
            {
                continue;
            }

            var vector = @object.GetTruePosition() - truePosition;
            var magnitude = vector.magnitude;
            if (!(magnitude <= distance) || PhysicsHelpers.AnyNonTriggersBetween(truePosition,
                vector.normalized,
                magnitude, LayerMask.GetMask("Ship", "Objects")))
            {
                continue;
            }

            result = @object;
            distance = magnitude;
        }
        return result;
    }
}