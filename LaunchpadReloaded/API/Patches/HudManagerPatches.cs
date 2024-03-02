using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    private static GameObject _bottomLeft;
    
    [HarmonyPostfix]
    [HarmonyPatch("Start")]
    public static void StartPostfix(HudManager __instance)
    {
        if (!_bottomLeft)
        {
            var buttons = __instance.transform.Find("Buttons");
            _bottomLeft = Object.Instantiate(buttons.Find("BottomRight").gameObject,buttons);
        }

        foreach (var t in _bottomLeft.GetComponentsInChildren<ActionButton>(true))
        {
            t.gameObject.Destroy();
        }
        
        var gridArrange = _bottomLeft.GetComponent<GridArrange>();
        var aspectPosition = _bottomLeft.GetComponent<AspectPosition>();

        _bottomLeft.name = "BottomLeft";
        gridArrange.Alignment = GridArrange.StartAlign.Right;
        aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftBottom;
        
        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.CreateButton(_bottomLeft.transform);
        }
        
        gridArrange.Start();
        gridArrange.ArrangeChilds();
        
        aspectPosition.AdjustPosition();
    }

    [HarmonyPostfix]
    [HarmonyPatch("SetHudActive",typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool))]
    public static void SetHudActivePostfix(HudManager __instance, [HarmonyArgument(0)] PlayerControl player, [HarmonyArgument(1)] RoleBehaviour roleBehaviour, [HarmonyArgument(2)] bool isActive)
    {
        if (player.Data == null)
        {
            return;
        }

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.SetActive(isActive, roleBehaviour);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer)
        {
            return;
        }

        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);
        }
    }
}