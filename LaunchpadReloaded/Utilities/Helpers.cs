using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        if (rend is null)
        {
            yield return null;
        }

        float alphaVal = rend!.color.a;
        Color tmp = rend.color;

        while (alphaVal > 0)
        {
            alphaVal -= decrease;
            tmp.a = alphaVal;
            rend!.color = tmp;

            yield return new WaitForSeconds(delay);
        }
    }

    private static readonly int _outline = Shader.PropertyToID("_Outline");
    private static readonly int _outlineColor = Shader.PropertyToID("_OutlineColor");
    private static readonly int _addColor = Shader.PropertyToID("_AddColor");

    /// Fixed version of Reactor's SetOutline
    public static void UpdateOutline(this Renderer renderer, Color? color)
    {
        renderer.material.SetFloat(_outline, color.HasValue ? 1 : 0);
        renderer.material.SetColor(_outlineColor, color.HasValue ? color.Value : Color.clear);
        renderer.material.SetColor(_addColor, color.HasValue ? color.Value : Color.clear);
    }

    public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[UnityEngine.Random.RandomRangeInt(0, s.Length)]).ToArray());
    }
}