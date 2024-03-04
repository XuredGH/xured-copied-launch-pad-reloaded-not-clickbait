﻿using HarmonyLib;
using LaunchpadReloaded.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(TaskPanelBehaviour))]
public static class TaskPanelPatch
{
    // This patch is to override the automatic updating of the y position on the tab, because I can't change the custom tab y pos if it's being overriden every frame.

    [HarmonyPrefix]
    [HarmonyPatch(nameof(TaskPanelBehaviour.Update))]
    public static bool UpdatePrefix(TaskPanelBehaviour __instance)
    {
        if (__instance.gameObject.name != "RolePanel") return true;
        __instance.background.transform.localScale = __instance.taskText.textBounds.size.x > 0f ? new Vector3(__instance.taskText.textBounds.size.x + 0.4f, __instance.taskText.textBounds.size.y + 0.3f, 1f) : Vector3.zero;
        Vector3 vector = __instance.background.sprite.bounds.extents;
        vector.y = -vector.y;
        vector = vector.Mul(__instance.background.transform.localScale);
        __instance.background.transform.localPosition = vector;
        Vector3 vector2 = __instance.tab.sprite.bounds.extents;
        vector2 = vector2.Mul(__instance.tab.transform.localScale);
        vector2.y = -vector2.y;
        vector2.x += vector.x * 2f;
        __instance.tab.transform.localPosition = vector2;
        if (GameManager.Instance == null)
        {
            return false;
        }
        Vector3 closePosition = new Vector3(-__instance.background.sprite.bounds.size.x * __instance.background.transform.localScale.x, __instance.closedPosition.y, __instance.closedPosition.z);
        __instance.closedPosition = closePosition;
        if (__instance.open)
        {
            __instance.timer = Mathf.Min(1f, __instance.timer + Time.deltaTime / __instance.animationTimeSeconds);
        }
        else
        {
            __instance.timer = Mathf.Max(0f, __instance.timer - Time.deltaTime / __instance.animationTimeSeconds);
        }
        Vector3 relativePos;
        relativePos = new Vector3(Mathf.SmoothStep(__instance.closedPosition.x, __instance.openPosition.x, __instance.timer), Mathf.SmoothStep(__instance.closedPosition.y, __instance.openPosition.y, __instance.timer), __instance.openPosition.z);
        __instance.transform.localPosition = AspectPosition.ComputePosition(AspectPosition.EdgeAlignments.LeftTop, relativePos);
        return false;
    }

}