using HarmonyLib;
using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(MeetingHud))]
public static class MeetingHudPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    public static void AwakePostfix()
    {
        DragManager.Instance.DraggingPlayers.Clear();
    }
}