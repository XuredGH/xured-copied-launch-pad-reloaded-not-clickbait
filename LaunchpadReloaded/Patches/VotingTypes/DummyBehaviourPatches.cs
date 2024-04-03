using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.VotingTypes;
[HarmonyPatch(typeof(DummyBehaviour))]
public static class DummyBehaviourPatches
{
    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void DummyUpdatePatch(DummyBehaviour __instance)
    {
        __instance.voted = __instance.myPlayer.GetLpPlayer().VotesRemaining == 0;
    }

    [HarmonyPostfix, HarmonyPatch("Start")]
    public static void DummyStartPatch(DummyBehaviour __instance)
    {
        if (LaunchpadSettings.Instance.UniqueDummies.Enabled) __instance.myPlayer.RpcSetName(AccountManager.Instance.GetRandomName());
    }
}