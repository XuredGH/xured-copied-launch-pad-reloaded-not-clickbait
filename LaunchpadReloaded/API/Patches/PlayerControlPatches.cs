using HarmonyLib;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(PlayerControl))]
public class PlayerControlPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("FixedUpdate")]
    public static void FixedUpdatePostfix(PlayerControl __instance)
    {
        if (__instance.Data.Role is ICustomRole customRole)
        {
            customRole.PlayerControlFixedUpdate(__instance);
        }
    }
}