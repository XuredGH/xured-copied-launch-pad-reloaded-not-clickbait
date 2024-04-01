using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using Reactor.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie)
    {
        tie = true;
        KeyValuePair<byte, int> result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
        foreach (KeyValuePair<byte, int> keyValuePair in self)
        {
            if (keyValuePair.Value > result.Value)
            {
                result = keyValuePair;
                tie = false;
            }
            else if (keyValuePair.Value == result.Value)
            {
                tie = true;
            }
        }
        return result;
    }

    /*    [MethodRpc((uint)LaunchpadRPC.VoteForPlayer)]
        public static void RpcVoteFor(this PlayerControl playerControl, byte playerId, bool edit = false, bool clear = false)
        {
            LaunchpadPlayer plr = playerControl.GetLpPlayer();

            if (clear)
            {
                plr.VotedPlayers.Clear();
                return;
            }

            if (edit)
            {
                plr.VotesRemaining = playerId;
                return;
            }

            plr.VotedPlayers.Add(playerId);
            if (playerId == 253) plr.VotesRemaining = 0; else plr.VotesRemaining -= 1;

            if (AmongUsClient.Instance.AmHost && MeetingHud.Instance)
            {
                MeetingHud.Instance.CheckForEndVoting();
            }

            *//*        if (LaunchpadGameOptions.Instance.LiveUpdating.Value && MeetingHud.Instance)
                    {
                        MeetingHud.Instance.PopulateResults(new MeetingHud.VoterState[MeetingHud.Instance.playerStates.Length]);
                    }*//*
        }

        public static void RpcSkipVote(this PlayerControl playerControl)
        {
            playerControl.RpcVoteFor(253);

            Debug.Log("Skipping vote for bomoclat ");
            if (playerControl.AmOwner)
            {
                Debug.Log("Blud i am owner");
                MeetingHud __instance = MeetingHud.Instance;
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    playerVoteArea.ClearButtons();
                    playerVoteArea.voteComplete = true;
                }

                __instance.SkipVoteButton.ClearButtons();
                __instance.SkipVoteButton.voteComplete = true;
                __instance.SkipVoteButton.gameObject.SetActive(false);
                __instance.state = MeetingHud.VoteStates.Voted;

                PlayerControl.LocalPlayer.RpcSendChatNote(PlayerControl.LocalPlayer.PlayerId, ChatNoteTypes.DidVote);
                SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false, 1f, null);

                TextMeshPro tmp = MeetingHudPatches.typeText.GetComponent<TextMeshPro>();
                tmp.text = LaunchpadPlayer.LocalPlayer.VotesRemaining + " votes left";
            }
        }
        public static void RpcEditVotes(this PlayerControl playerControl, byte value) => playerControl.RpcVoteFor(value, edit: true);
        public static void RpcClearVote(this PlayerControl playerControl) => playerControl.RpcVoteFor(0, clear: true);*/


    public static LaunchpadPlayer GetLpPlayer(this PlayerControl playerControl)
    {
        return playerControl.gameObject.GetComponent<LaunchpadPlayer>();
    }

    public static bool ButtonTimerEnabled(this PlayerControl playerControl)
    {
        return (playerControl.moveable || playerControl.petting) && !playerControl.inVent && !playerControl.shapeshifting && (!DestroyableSingleton<HudManager>.InstanceExists || !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance && !IntroCutscene.Instance;
    }

    public static bool IsHacked(this GameData.PlayerInfo playerInfo)
    {
        if (!HackingManager.Instance) return false;

        return HackingManager.Instance.hackedPlayers.Contains(playerInfo.PlayerId);
    }

    public static bool IsRevived(this PlayerControl player)
    {
        if (RevivalManager.Instance is null)
        {
            return false;
        }

        return RevivalManager.Instance.RevivedPlayers.Contains(player.PlayerId);
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
        var results = new Il2CppSystem.Collections.Generic.List<Collider2D>();
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