using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Buttons;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer) return;

        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(HudManager __instance)
    {
        var bottomLeft = Object.Instantiate(__instance.transform.Find("Buttons").Find("BottomRight").gameObject,__instance.transform.Find("Buttons"));

        foreach (var t in bottomLeft.GetComponentsInChildren<ActionButton>(true))
        {
            t.gameObject.Destroy();
        }
        
        var gridArrange = bottomLeft.GetComponent<GridArrange>();
        var aspectPosition = bottomLeft.GetComponent<AspectPosition>();

        bottomLeft.name = "BottomLeft";
        gridArrange.Alignment = GridArrange.StartAlign.Right;
        aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftBottom;
        
        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.CreateButton(bottomLeft.transform);
        }
        
        gridArrange.Start();
        gridArrange.ArrangeChilds();
        
        aspectPosition.AdjustPosition();
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("SetHudActive")]
    [HarmonyPatch([typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool)])]
    public static void SetHudActivePostfix(HudManager __instance, [HarmonyArgument(1)] RoleBehaviour roleBehaviour, [HarmonyArgument(2)] bool isActive)
    {
        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.SetActive(isActive, roleBehaviour);
        }
    }
}