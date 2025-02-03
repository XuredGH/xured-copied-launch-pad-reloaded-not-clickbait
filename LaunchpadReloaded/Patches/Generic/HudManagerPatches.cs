using HarmonyLib;
using InnerNet;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using MiraAPI.Hud;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.Patches.Generic;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    public static GameObject notepadButton;
    public static GameObject notepad;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(HudManager.Start))]
    public static void StartPostfix(HudManager __instance)
    {
        notepad = Object.Instantiate(LaunchpadAssets.Notepad.LoadAsset(), HudManager.Instance.transform);
        notepad.gameObject.SetActive(false);

        var tmpTextBox = notepad.GetComponentInChildren<TextBoxTMP>();
        var psBtn = notepad.GetComponentInChildren<PassiveButton>();

        psBtn.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        psBtn.OnClick.AddListener((UnityAction)(() =>
        {
            tmpTextBox.GiveFocus();
        }));

        notepadButton = Object.Instantiate(__instance.SettingsButton.gameObject, __instance.SettingsButton.transform.parent);
        notepadButton.name = "NotepadButton";

        var aspectPos = notepadButton.GetComponent<AspectPosition>();
        aspectPos.DistanceFromEdge += new Vector3(0.86f, 0);
        aspectPos.AdjustPosition();

        var inactiveSprite = notepadButton.transform.FindChild("Inactive").GetComponent<SpriteRenderer>();
        inactiveSprite.sprite = LaunchpadAssets.NotepadSprite.LoadAsset();
        inactiveSprite.transform.localPosition = new Vector3(0.005f, 0.025f, 0f);

        var activeSprite = notepadButton.transform.FindChild("Active").GetComponent<SpriteRenderer>();
        activeSprite.sprite = LaunchpadAssets.NotepadActiveSprite.LoadAsset();
        activeSprite.transform.localPosition = inactiveSprite.transform.localPosition;

        var passiveButton = notepadButton.GetComponent<PassiveButton>();
        passiveButton.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
        passiveButton.OnClick.AddListener((UnityAction)(() =>
        {
            notepad.gameObject.SetActive(!notepad.gameObject.active);
        }));
    }
    /// <summary>
    /// Generic update method for most of HUD logic in Launchpad
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(HudManager.Update))]
    public static void UpdatePostfix(HudManager __instance)
    {
        var local = PlayerControl.LocalPlayer;
        if (!local || MeetingHud.Instance)
        {
            if (ZoomButton.IsZoom)
            {
                CustomButtonSingleton<ZoomButton>.Instance.OnEffectEnd();
            }

            return;
        }

        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started && !ShipStatus.Instance)
        {
            return;
        }

        if (!(local.Data.IsHacked() && !local.Data.Role.IsImpostor) && HackerUtilities.AnyPlayerHacked())
        {
            var newB = new StringBuilder();
            newB.Append(Color.green.ToTextColor());
            newB.Append(local.Data.Role.IsImpostor ?
                "\n\n The crewmates are hacked! They will not be able to\ncomplete tasks or call meetings until they reverse the hack."
                : "\n\nYou will still not be able to report bodies or \ncall meetings until all crewmates reverse the hack.");
            newB.Append($"\n{HackerUtilities.CountHackedPlayers()} players are still hacked.");
            newB.Append("</color>");
            __instance.TaskPanel.SetTaskText(__instance.tasksString.ToString() + newB);
        }

        if (HackerUtilities.AnyPlayerHacked())
        {
            __instance.ReportButton.SetActive(false);
        }
    }
}