﻿using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Roles.Medic;

/// <summary>
/// Disable chat if revived
/// </summary>
[HarmonyPatch(typeof(ChatController))]
public static class ChatPatches
{
    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void UpdatePatch(ChatController __instance)
    {
        if (!LaunchpadPlayer.LocalPlayer.wasRevived)
        {
            return;
        }

        if (!__instance.freeChatField)
        {
            return;
        }

        __instance.sendRateMessageText.gameObject.SetActive(true);
        __instance.sendRateMessageText.text = "You have been revived. You can no longer speak.";
        __instance.sendRateMessageText.color = LaunchpadPalette.MedicColor;
        __instance.quickChatButton.gameObject.SetActive(false);
        __instance.freeChatField.textArea.gameObject.SetActive(false);
        __instance.openKeyboardButton.gameObject.SetActive(false);
    }
    
    [HarmonyPrefix, HarmonyPatch(nameof(ChatController.SendChat))]
    public static bool SendChatPatch()
    {
        return !LaunchpadPlayer.LocalPlayer.wasRevived;
    }
    
    [HarmonyPrefix, HarmonyPatch(nameof(ChatController.AddChat))]
    public static bool AddChatPatch([HarmonyArgument(0)] PlayerControl player)
    {
        return !player.GetLpPlayer().wasRevived;
    }
}