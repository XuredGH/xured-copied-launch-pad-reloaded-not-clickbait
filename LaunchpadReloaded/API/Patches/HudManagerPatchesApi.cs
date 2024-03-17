using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatchesApi
{
    private static GameObject _bottomLeft;
    public static TaskPanelBehaviour _roleTab;

    private static float _increment = 0.3f;
    private static FloatRange _bounds = new FloatRange(2.9f, 3.5f);

    [HarmonyPostfix]
    [HarmonyPatch("Update")]
    public static void UpdatePostfix(HudManager __instance)
    {
        if (!PlayerControl.LocalPlayer)
        {
            return;
        }

        if (!ShipStatus.Instance)
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

        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started && !ShipStatus.Instance)
        {
            return;
        }

        CustomGameModeManager.ActiveMode.HudUpdate(__instance);

        if (PlayerControl.LocalPlayer.Data.Role is ICustomRole customRole)
        {
            customRole.HudUpdate(__instance);

            if (PlayerControl.LocalPlayer.Data.IsHacked())
            {
                if (_roleTab)
                {
                    _roleTab.gameObject.Destroy();
                }

                return;
            }

            if (customRole.SetTabText() != null)
            {
                if (_roleTab == null)
                {
                    _roleTab = CustomRoleManager.CreateRoleTab(customRole);
                }
                else
                {
                    CustomRoleManager.UpdateRoleTab(_roleTab, customRole);
                }
            }
        }
        else
        {
            // If not custom role and roleTab exists delete roletab (could happen in freeplay mode if switching roles)
            if (_roleTab != null)
            {
                _roleTab.gameObject.Destroy();
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("OnGameStart")]
    public static void GameStartPatch(HudManager __instance)
    {
        CustomGameModeManager.ActiveMode.HudStart(__instance);
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

        if (_roleTab)
        {
            _roleTab.gameObject.SetActive(isActive);
        }

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            button.SetActive(isActive, roleBehaviour);
        }

        if (roleBehaviour is ICustomRole role)
        {
            __instance.ImpostorVentButton.gameObject.SetActive(isActive && role.CanUseVent);
        }
    }
}