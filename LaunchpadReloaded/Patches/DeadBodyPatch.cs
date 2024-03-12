using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(DeadBody))]
public static class DeadBodyPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(DeadBody.OnClick))]
    public static bool OnClickPatch()
    {
        return !HackingManager.Instance.AnyActiveNodes();
    }
}
