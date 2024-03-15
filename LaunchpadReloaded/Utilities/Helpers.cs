using System;
using System.Linq;
using System.Text;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static Random Random = new Random();

    public static bool ShouldCancelClick()
    {
        return DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId);
    }

    public static DeadBody GetBodyById(byte id)
    {
        return UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(body => body.ParentId == id);
    }

    public static TextMeshPro CreateTextLabel(string name, Transform parent,
        AspectPosition.EdgeAlignments alignment, Vector3 distance, float fontSize = 2f,
        TextAlignmentOptions textAlignment = TextAlignmentOptions.Center)
    {
        var textObj = new GameObject(name);
        textObj.transform.parent = parent;
        textObj.layer = LayerMask.NameToLayer("UI");

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
        var notifs = HudManager.Instance.Notifier.transform.parent.FindChild("LaunchpadNotifications").GetComponent<NotificationPopper>();
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
        var taskStringBuilder = new StringBuilder();
        taskStringBuilder.AppendLine($"{role.RoleColor.ToTextColor()}You are a <b>{role.RoleName}.</b></color>");
        taskStringBuilder.Append("<size=70%>");
        taskStringBuilder.AppendLine($"{role.RoleLongDescription}");
        return taskStringBuilder;
    }
}