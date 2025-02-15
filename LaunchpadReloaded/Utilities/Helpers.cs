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
    public static void AddMessage(string item, Sprite spr, AudioClip clip, Color color, out LobbyNotificationMessage msg)
    {
        NotificationPopper popper = HudManager.Instance.Notifier;
        LobbyNotificationMessage newMessage = GameObject.Instantiate(popper.notificationMessageOrigin, Vector3.zero, Quaternion.identity, popper.transform);
        newMessage.transform.localPosition = new Vector3(0f, 0f, -2f);
        newMessage.SetUp(item, spr, color, new System.Action(() => popper.OnMessageDestroy(newMessage)));
        popper.lastMessageKey = -1;
        popper.ShiftMessages();
        popper.AddMessageToQueue(newMessage);
        msg = newMessage;
        if (clip != null)
        {
            SoundManager.Instance.StopSound(clip);
            SoundManager.Instance.PlaySound(clip, false);
        }
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

    public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[UnityEngine.Random.RandomRangeInt(0, s.Length)]).ToArray());
    }
}