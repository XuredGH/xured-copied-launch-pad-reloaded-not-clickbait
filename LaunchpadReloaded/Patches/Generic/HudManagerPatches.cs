﻿using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Patches.Options;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using System.Text;
using LaunchpadReloaded.Features.Managers;
using UnityEngine;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    /// Custom buttons parent
    private static GameObject _bottomLeft;
    /// Custom role tab
    public static TaskPanelBehaviour _roleTab;
    /// Scrolling increment
    private static float _increment = 0.3f;
    /// Bounds for scrolling 
    private static FloatRange _bounds = new FloatRange(2.9f, 4.6f);

    /// <summary>
    /// Add options scrolling (on the hud text)
    /// </summary>
    public static void OptionsScrollingLogic(HudManager __instance)
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToHudStringPatch.ShowCustom = !ToHudStringPatch.ShowCustom;
        }

        var numPlayers = GameData.Instance ? GameData.Instance.PlayerCount : 10;
        HudManager.Instance.GameSettings.text = GameOptionsManager.Instance.CurrentGameOptions.ToHudString(numPlayers);

        if (!PlayerControl.LocalPlayer.CanMove) return;
        if (!ToHudStringPatch.ShowCustom) return;

        var pos = __instance.GameSettings.transform.localPosition;
        if (Input.mouseScrollDelta.y > 0f)
        {
            pos =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y - _increment, _bounds.min, _bounds.max), pos.z);
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            pos =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y + _increment, _bounds.min, _bounds.max), pos.z);
        }

        __instance.GameSettings.transform.localPosition = pos;
    }

    /// <summary>
    /// Add the "hacked" task string if local player is hacked
    /// </summary>
    public static void AddHackedTaskString(HudManager __instance)
    {
        __instance.tasksString.Clear();
        __instance.tasksString.Append(Color.green.ToTextColor());
        __instance.tasksString.Append("You have been hacked!\n");
        __instance.tasksString.Append("You are unable to complete tasks or call meetings.\n");
        __instance.tasksString.Append("Find an active node to reverse the hack!.\n");
        __instance.tasksString.Append($"{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
        __instance.tasksString.Append("</color>");
        __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString());

        if (_roleTab != null) _roleTab.gameObject.Destroy();
    }

    /// <summary>
    /// Generic update method for most of HUD logic in Launchpad
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        var local = PlayerControl.LocalPlayer;
        if (!local || MeetingHud.Instance) return;

        if (!ShipStatus.Instance) OptionsScrollingLogic(__instance);

        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started && !ShipStatus.Instance) return;

        CustomGameModeManager.ActiveMode.HudUpdate(__instance);

        if (local.Data.IsHacked()) AddHackedTaskString(__instance);
        else if (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes())
        {
            var newB = new StringBuilder();
            newB.Append(Color.green.ToTextColor());
            newB.Append(local.Data.Role is HackerRole ?
                "\n\nYou have hacked the crewmates! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
            newB.Append($"\n{HackingManager.Instance.hackedPlayers.Count} players are still hacked.");
            newB.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB);
        }

        if (local.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);

            if (PlayerControl.LocalPlayer.Data.IsHacked())
            {
                if (_roleTab) _roleTab.gameObject.Destroy();

                return;
            }

            if (customRole.SetTabText() != null)
            {
                if (_roleTab == null) _roleTab = CustomRoleManager.CreateRoleTab(customRole);
                else CustomRoleManager.UpdateRoleTab(_roleTab, customRole);
            }
        }
        else if (_roleTab != null) _roleTab.gameObject.Destroy();

        if (DragManager.Instance is null || HackingManager.Instance is null) return;
        if (HackingManager.Instance.AnyActiveNodes()) __instance.ReportButton.SetDisabled();

        foreach (var (player, bodyId) in DragManager.Instance.DraggingPlayers)
        {
            var bodyById = DeadBodyManager.GetBodyById(bodyId);
            bodyById.transform.position = Vector3.Lerp(bodyById.transform.position, GameData.Instance.GetPlayerById(player).Object.transform.position, 5f * Time.deltaTime);
        }
    }

    /// <summary>
    /// Trigger hudstart on current custom gamemode
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("OnGameStart")]
    public static void GameStartPatch(HudManager __instance)
    {
        CustomGameModeManager.ActiveMode.HudStart(__instance);
    }

    /// <summary>
    /// Create custom buttons parent
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("Start")]
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

    /// <summary>
    /// Make sure all launchpad hud elements are inactive/active when appropriate
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("SetHudActive", typeof(PlayerControl), typeof(RoleBehaviour), typeof(bool))]
    public static void SetHudActivePostfix(HudManager __instance, [HarmonyArgument(0)] PlayerControl player, [HarmonyArgument(1)] RoleBehaviour roleBehaviour, [HarmonyArgument(2)] bool isActive)
    {
        if (player.Data == null) return;
        if (_roleTab) _roleTab.gameObject.SetActive(isActive);

        foreach (var button in CustomButtonManager.CustomButtons)
            button.SetActive(isActive, roleBehaviour);

        if (roleBehaviour is ICustomRole role)
            __instance.ImpostorVentButton.gameObject.SetActive(isActive && role.CanUseVent);
    }
}