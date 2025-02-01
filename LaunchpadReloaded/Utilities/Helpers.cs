﻿using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static readonly Random Random = new();

    public static List<PlayerControl> GetAlivePlayers()
    {
        return PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.IsDead).ToList();
    }

    public static bool ShouldCancelClick()
    {
        return PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>() || PlayerControl.LocalPlayer.GetModifier<HackedModifier>() is { DeActivating: false };
    }

    public static string FirstLetterToUpper(string str)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
    }

    public static IEnumerator FadeOut(SpriteRenderer rend, float delay = 0.01f, float decrease = 0.01f)
    {
        float alphaVal = rend.color.a;
        Color tmp = rend.color;

        while (alphaVal > 0)
        {
            alphaVal -= decrease;
            tmp.a = alphaVal;
            rend.color = tmp;

            yield return new WaitForSeconds(delay);
        }
    }

    public static TextMeshPro CreateTextLabel(string name, Transform parent,
        AspectPosition.EdgeAlignments alignment, Vector3 distance, float fontSize = 2f,
        TextAlignmentOptions textAlignment = TextAlignmentOptions.Center)
    {
        var textObj = new GameObject(name)
        {
            transform =
            {
                parent = parent
            },
            layer = LayerMask.NameToLayer("UI")
        };

        var pingTracker = HudManager.Instance.gameObject.GetComponentInChildren<PingTracker>();
        var textMeshPro = textObj.AddComponent<TextMeshPro>();
        textMeshPro.fontSize = fontSize;
        textMeshPro.alignment = textAlignment;
        textMeshPro.fontMaterial = pingTracker.text.fontMaterial;

        var aspectPosition = textObj.AddComponent<AspectPosition>();
        aspectPosition.Alignment = alignment;
        aspectPosition.DistanceFromEdge = distance;
        aspectPosition.AdjustPosition();

        return textMeshPro;
    }

    public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[UnityEngine.Random.RandomRangeInt(0, s.Length)]).ToArray());
    }
}