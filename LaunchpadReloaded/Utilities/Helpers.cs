using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static System.Random Random = new System.Random();

    public static bool ShouldCancelClick()
    {
        return DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId);
    }

    public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static string GetSuffix(NumberSuffixes suffix)
    {
        switch (suffix)
        {
            case NumberSuffixes.None:
                return String.Empty;
            case NumberSuffixes.Multiplier:
                return "x";
            case NumberSuffixes.Seconds:
                return "s";
        }

        return String.Empty;
    }

    public static void SendNotification(string text, Color textColor, float duration = 2f, float fontSize = 3f)
    {
        NotificationPopper notifs = HudManager.Instance.Notifier.transform.parent.FindChild("LaunchpadNotifications").GetComponent<NotificationPopper>();
        notifs.TextArea.text = text;
        notifs.TextArea.fontSize = fontSize;
        notifs.textColor = textColor;
        notifs.alphaTimer = duration;
    }

    public static PlainShipRoom GetRoom(Vector3 pos)
    {
        return ShipStatus.Instance.AllRooms.ToList().Find(room => room.roomArea.OverlapPoint(pos));
    }

    public static StringBuilder CreateForRole(ICustomRole role)
    {
        StringBuilder taskStringBuilder = new StringBuilder();
        taskStringBuilder.AppendLine($"{role.RoleColor.ToTextColor()}You are a <b>{role.RoleName}.</b></color>");
        taskStringBuilder.Append("<size=70%>");
        taskStringBuilder.AppendLine($"{role.RoleLongDescription}");
        return taskStringBuilder;
    }
}