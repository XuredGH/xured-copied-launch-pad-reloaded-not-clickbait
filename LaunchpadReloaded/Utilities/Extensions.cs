using System;
using System.Linq;
using System.Reflection;
using AmongUs.GameOptions;
using System.Collections.Generic;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using PowerTools;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.Utilities;

public static class Extensions
{
    private static readonly ContactFilter2D Filter = ContactFilter2D.CreateLegacyFilter(Constants.NotShipMask, float.MinValue, float.MaxValue);

    
    public static void SetBodyType(this PlayerControl player, int bodyType)
    {
        if (bodyType == 6)
        {
            player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
            if (!OptionGroupSingleton<BattleRoyaleOptions>.Instance.ShowKnife.Value)
            {
                return;
            }

            var seekerHand = player.transform.FindChild("BodyForms/Seeker/SeekerHand").gameObject;
            var hand = Object.Instantiate(seekerHand).gameObject;
            hand.transform.SetParent(seekerHand.transform.parent);
            hand.transform.localScale = new Vector3(2, 2, 2);
            hand.name = "KnifeHand";
            hand.layer = LayerMask.NameToLayer("Players");

            var transform = player.transform;

            hand.transform.localPosition = transform.localPosition;
            hand.transform.position = transform.position;

            var nodeSync = hand.GetComponent<SpriteAnimNodeSync>();
            nodeSync.flipOffset = new Vector3(-1.5f, 0.5f, 0);
            nodeSync.normalOffset = new Vector3(1.5f, 0.5f, 0);

            var rend = hand.GetComponent<SpriteRenderer>();
            rend.sprite = LaunchpadAssets.KnifeHandSprite.LoadAsset();

            hand.SetActive(true);
            return;
        }

        player.MyPhysics.SetBodyType((PlayerBodyTypes)bodyType);
        
        if (bodyType == (int)PlayerBodyTypes.Normal)
        {
            player.cosmetics.currentBodySprite.BodySprite.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            player.cosmetics.SetNamePosition(new Vector3(0, 1, -.5f));
        }
    }
    
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
        var result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
        foreach (var keyValuePair in self)
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

    public static KeyValuePair<byte, float> MaxPair(this Dictionary<byte, float> self, out bool tie)
    {
        tie = true;
        var result = new KeyValuePair<byte, float>(byte.MaxValue, int.MinValue);
        foreach (var keyValuePair in self)
        {
            if (keyValuePair.Value > result.Value)
            {
                result = keyValuePair;
                tie = false;
            }
            else if (Math.Abs(keyValuePair.Value - result.Value) < .05)
            {
                tie = true;
            }
        }
        return result;
    }

    public static bool ButtonTimerEnabled(this PlayerControl playerControl)
    {
        return (playerControl.moveable || playerControl.petting) && playerControl is { inVent: false, shapeshifting: false } && (!DestroyableSingleton<HudManager>.InstanceExists || !DestroyableSingleton<HudManager>.Instance.IsIntroDisplayed) && !MeetingHud.Instance && !PlayerCustomizationMenu.Instance && !ExileController.Instance && !IntroCutscene.Instance;
    }

    public static bool IsHacked(this NetworkedPlayerInfo playerInfo)
    {
        if (!HackingManager.Instance)
        {
            return false;
        }

        return HackingManager.Instance.hackedPlayers.Contains(playerInfo.PlayerId) || (playerInfo.Role.IsImpostor && HackingManager.Instance.AnyPlayerHacked());
    }

    public static void Revive(this DeadBody body)
    {
        var player = PlayerControl.AllPlayerControls.ToArray().ToList().Find(player => player.PlayerId == body.ParentId);
        if (player == null)
        {
            return;
        }

        player.NetTransform.SnapTo(body.transform.position);
        body.gameObject.Destroy();
        player.GetModifierComponent()!.AddModifier<RevivedModifier>();
    }
    
    public static void HideBody(this DeadBody body)
    {
        body.Reported = true;
        body.enabled = false;
        foreach (var spriteRenderer in body.bodyRenderers)
        {
            spriteRenderer.enabled = false;
        }
        body.GetComponent<DeadBodyComponent>().hidden = true;
    }

    public static void ShowBody(this DeadBody body, bool reported)
    {
        body.Reported = reported;
        body.enabled = true;
        foreach (var spriteRenderer in body.bodyRenderers)
        {
            spriteRenderer.enabled = true;
        }
        body.GetComponent<DeadBodyComponent>().hidden = false;
    }
    
    public static bool IsOverride(this MethodInfo methodInfo)
    {
        return methodInfo.GetBaseDefinition() != methodInfo;
    }
}