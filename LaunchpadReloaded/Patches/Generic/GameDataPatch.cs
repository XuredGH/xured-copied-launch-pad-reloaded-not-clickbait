using HarmonyLib;
using LaunchpadReloaded.Components;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(GameData),nameof(GameData.Awake))]
public static class GameDataPatch
{
    public static void Postfix(GameData __instance)
    {
        __instance.gameObject.AddComponent<CustomGameData>();
    }
}