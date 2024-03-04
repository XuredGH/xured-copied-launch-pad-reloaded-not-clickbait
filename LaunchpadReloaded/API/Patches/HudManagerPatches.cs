using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using static UnityEngine.RemoteConfigSettingsHelper;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    private static GameObject _bottomLeft;
    private static TaskPanelBehaviour _roleTab;

    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer) return;

        if(PlayerControl.LocalPlayer.Data.IsHacked())
        {
            __instance.tasksString.Clear();
            __instance.tasksString.Append(UnityEngine.Color.green.ToTextColor());
            __instance.tasksString.Append("You have been hacked!\n");
            __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
            __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
            __instance.tasksString.Append("</color>");

            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());

            if (_roleTab != null) _roleTab.gameObject.Destroy();
        }

        if (HackingManager.AnyActiveNodes()) __instance.ReportButton.SetDisabled();

        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);

            if (PlayerControl.LocalPlayer.Data.IsHacked())
            {
                if(_roleTab) _roleTab.gameObject.Destroy();
                return;
            }

            if (customRole.SetTabText() != null)
            {
                if (_roleTab == null)
                    _roleTab = CustomRoleManager.CreateRoleTab(customRole);
                else
                    CustomRoleManager.UpdateRoleTab(_roleTab, customRole);
            }
        }
        else
        {
            // If not custom role and roleTab exists delete roletab (could happen in freeplay mode if switching roles)
            if (_roleTab != null) _roleTab.gameObject.Destroy();
        }
    }
    
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
}