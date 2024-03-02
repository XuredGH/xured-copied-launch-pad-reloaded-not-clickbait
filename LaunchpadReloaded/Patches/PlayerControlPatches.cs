using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.IsKillTimerEnabled),MethodType.Getter)]
    public static void GetKillTimerEnabledPostfix(PlayerControl __instance, ref bool __result)
    {
        switch (__instance.Data.Role)
        {
            case JanitorRole:
                __result = __result && !DragManager.IsDragging(__instance.PlayerId);
                break;
        }
    }
}