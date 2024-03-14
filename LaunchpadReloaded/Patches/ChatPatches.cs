using HarmonyLib;
using LaunchpadReloaded.API.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(ChatController))]
public static class ChatPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ChatController.Update))]
    [HarmonyPatch(nameof(ChatController.SendChat))]
    public static void CantChatPatch(ChatController __instance)
    {
        if (!PlayerControl.LocalPlayer.IsRevived()) return;

        __instance.sendRateMessageText.gameObject.SetActive(true);
        __instance.sendRateMessageText.text = "You have been revived. You can no longer speak.";
    }
}