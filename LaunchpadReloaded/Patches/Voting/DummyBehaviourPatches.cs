using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Patches.Voting;
[HarmonyPatch(typeof(DummyBehaviour))]
public static class DummyBehaviourPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(DummyBehaviour.Update))]
    public static void DummyUpdatePatch(DummyBehaviour __instance)
    {
        if (MeetingHud.Instance && __instance.myPlayer.HasModifier<VoteData>())
        {
            __instance.voted = __instance.myPlayer.GetModifier<VoteData>()!.VotesRemaining == 0;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(DummyBehaviour.Start))]
    public static void DummyStartPatch(DummyBehaviour __instance)
    {
        __instance.myPlayer.RpcSetRole(RoleTypes.Crewmate, false);

        if (LaunchpadSettings.Instance?.UniqueDummies.Enabled == true)
        {
            __instance.myPlayer.RpcSetName(AccountManager.Instance.GetRandomName());
        }
    }
}