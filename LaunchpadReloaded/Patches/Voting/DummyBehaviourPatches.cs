using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Patches.Voting;
[HarmonyPatch(typeof(DummyBehaviour))]
public static class DummyBehaviourPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DummyBehaviour.Update))]
    public static void DummyUpdatePatch(DummyBehaviour __instance)
    {
        __instance.voted = __instance.myPlayer.GetModifier<VoteData>().VotesRemaining == 0;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(DummyBehaviour.Start))]
    public static void DummyStartPatch(DummyBehaviour __instance)
    {
        if (LaunchpadSettings.Instance.UniqueDummies.Enabled)
        {
            __instance.myPlayer.RpcSetName(AccountManager.Instance.GetRandomName());
        }
    }
}