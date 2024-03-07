using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("RpcSyncSettings")]
    public static void RpcSyncSettingsPostfix(PlayerControl __instance)
    {
        CustomOptionsManager.SyncOptions();
    }
    
    
    [HarmonyPostfix]
    [HarmonyPatch("FixedUpdate")]
    public static void FixedUpdatePostfix(PlayerControl __instance)
    {
        if (!__instance.AmOwner || MeetingHud.Instance)
        {
            return;
        }

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            if (button.Enabled(__instance.Data.Role))
            {
                button.UpdateHandler(__instance);
            }
        }
        
        if (__instance.Data.Role is ICustomRole customRole)
        {
            customRole.PlayerControlFixedUpdate(__instance);
        }
    }
}