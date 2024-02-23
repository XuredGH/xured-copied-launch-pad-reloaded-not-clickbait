using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer) return;
        CustomButton.UpdateButtons();
        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(HudManager __instance)
    {
        foreach (var button in CustomButton.AllButtons)
        {
            button.VerifyButton();
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("SetHudActive")]
    [HarmonyPatch([typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool)])]
    public static void SetHudActivePostfix(HudManager __instance, [HarmonyArgument(1)] RoleBehaviour roleBehaviour, [HarmonyArgument(2)] bool isActive)
    {
        foreach (var button in CustomButton.AllButtons)
        {
            button.SetHudActive(__instance, roleBehaviour, isActive);
        }
    }
}