using HarmonyLib;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(ChatController))]
public static class ChatPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ChatController.Update))]
    public static void UpdatePatch(ChatController __instance)
    {
        if (!PlayerControl.LocalPlayer.IsRevived())
        {
            return;
        }

        if (__instance.freeChatField is null)
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
}