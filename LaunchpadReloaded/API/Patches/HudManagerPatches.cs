using HarmonyLib;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using Reactor.Utilities.Extensions;
using System.Text;
using UnityEngine;

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
        if (!ShipStatus.Instance)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                ToHudStringPatch.ShowCustom = !ToHudStringPatch.ShowCustom;

            var numPlayers = GameData.Instance ? GameData.Instance.PlayerCount : 10;
            HudManager.Instance.GameSettings.text = GameOptionsManager.Instance.CurrentGameOptions.ToHudString(numPlayers);
        }

        if (!PlayerControl.LocalPlayer) return;

        if (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            __instance.tasksString.Clear();
            __instance.tasksString.Append(UnityEngine.Color.green.ToTextColor());
            __instance.tasksString.Append("You have been hacked!\n");
            __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
            __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
            __instance.tasksString.Append($"{HackingManager.Instance.HackedPlayers.Count} players are still hacked.");
            __instance.tasksString.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());

            if (_roleTab != null) _roleTab.gameObject.Destroy();
        }
        else if (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes())
        {
            StringBuilder newB = new StringBuilder();
            newB.Append(UnityEngine.Color.green.ToTextColor());
            newB.Append(PlayerControl.LocalPlayer.Data.Role is HackerRole ?
                "\n\nYou have hacked the crewmates! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
            newB.Append($"\n{HackingManager.Instance.HackedPlayers.Count} players are still hacked.");
            newB.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB.ToString());
        }

        if (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes()) __instance.ReportButton.SetDisabled();

        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);

            if (PlayerControl.LocalPlayer.Data.IsHacked())
            {
                if (_roleTab) _roleTab.gameObject.Destroy();
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
            _bottomLeft = Object.Instantiate(buttons.Find("BottomRight").gameObject, buttons);
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
    [HarmonyPatch("SetHudActive", typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool))]
    public static void SetHudActivePostfix(HudManager __instance, [HarmonyArgument(0)] PlayerControl player, [HarmonyArgument(1)] RoleBehaviour roleBehaviour, [HarmonyArgument(2)] bool isActive)
    {
        if (player.Data == null)
        {
            return;
        }

        if (_roleTab) _roleTab.gameObject.SetActive(isActive);

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.SetActive(isActive, roleBehaviour);
        }
    }
}