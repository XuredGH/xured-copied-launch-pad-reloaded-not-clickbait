using LaunchpadReloaded.Features;
using System.Diagnostics.Metrics;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class Helpers
{
    public static bool ShouldCancelClick()
    {
        return DragManager.IsDragging(PlayerControl.LocalPlayer.PlayerId);
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
}