using AmongUs.Data;
using HarmonyLib;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(PlayerTab))]
public static class PlayerTabPatches
{
    private static bool _selectGradient;
    private static ColorChip _switchButton;
    
    private static void SwitchSelector(PlayerTab instance)
    {
        _selectGradient = !_selectGradient;
        _switchButton.GetComponentInChildren<TextMeshPro>().text = _selectGradient ? "Gradient" : "Base Color";
        if (_selectGradient)
        {
            instance.currentColor = GradientColorManager.Instance.LocalColorId;
        }
    }
    
    
    [HarmonyPostfix]
    [HarmonyPatch("OnEnable")]
    public static void OnEnablePostfix(PlayerTab __instance)
    {
        if (!_switchButton)
        {
            _switchButton = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea);
        
            var spriteRenderer = _switchButton.GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite = LaunchpadReloadedPlugin.BlankButton;
        
            _switchButton.GetComponent<BoxCollider2D>().size = sprite.rect.size/sprite.pixelsPerUnit;
            _switchButton.transform.localScale = new Vector3(1, 1, 1);
            _switchButton.transform.localPosition = new Vector3(2, 1.5f, -2);
            
            var buttonText = Object.Instantiate(__instance.transform.Find("Text").gameObject,_switchButton.transform);
            buttonText.transform.localPosition = new Vector3(0, 0, 0);
            buttonText.GetComponent<TextTranslatorTMP>().Destroy();
            
            var tmp = buttonText.GetComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.text = _selectGradient ? "Gradient" : "Base Color";
            tmp.fontSize = tmp.fontSizeMax = 4;
        
            _switchButton.Button.OnClick.RemoveAllListeners();
            _switchButton.Button.OnMouseOut.RemoveAllListeners();
            _switchButton.Button.OnMouseOver.RemoveAllListeners();
            _switchButton.Button.OnClick.AddListener((UnityAction)(() => { SwitchSelector(__instance); }));
        }


    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.ClickEquip))]
    public static bool ClickPrefix(PlayerTab __instance)
    {
        if (_selectGradient)
        {
            GradientColorManager.Instance.LocalColorId = __instance.currentColor;
            __instance.PlayerPreview.UpdateFromDataManager(PlayerMaterial.MaskType.None);
            if (__instance.HasLocalPlayer())
            {
                GradientColorManager.RpcSetGradient(PlayerControl.LocalPlayer, __instance.currentColor);
            }
            
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.SelectColor))]
    public static bool SelectPrefix(PlayerTab __instance, [HarmonyArgument(0)] int colorId)
    {
        if (_selectGradient)
        {
            __instance.UpdateAvailableColors();
            __instance.currentColor = colorId;
            if (colorId == DataManager.Player.Customization.Color)
            {
                __instance.currentColor = GradientColorManager.Instance.LocalColorId;
            }
            var colorName = Palette.GetColorName(colorId);
            PlayerCustomizationMenu.Instance.SetItemName(colorName);
            __instance.PlayerPreview.UpdateFromDataManager(PlayerMaterial.MaskType.None);
            return false;
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.GetCurrentColorId))]
    public static bool GetCurrentColorPrefix(PlayerTab __instance, ref int __result)
    {
        if (_selectGradient)
        {
            __result = GradientColorManager.Instance.LocalColorId;
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerTab.Update))]
    public static void UpdatePostfix(PlayerTab __instance)
    {
        if (_selectGradient)
        {
            __instance.currentColorIsEquipped = __instance.currentColor == GradientColorManager.Instance.LocalColorId;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.UpdateAvailableColors))]
    public static bool UpdateColorsPrefix(PlayerTab __instance)
    {
        if (_selectGradient)
        {
            for (var i = 0; i < Palette.PlayerColors.Length; i++)
            {
                __instance.AvailableColors.Add(i);
            }
            
            if (GameData.Instance)
            {
                var allPlayers = GameData.Instance.AllPlayers.ToArray();
                var grads = GradientColorManager.Instance.Gradients;
                var localGradId = GradientColorManager.Instance.LocalColorId;
                var localColorId = PlayerControl.LocalPlayer.Data.DefaultOutfit.ColorId;
                for (var j = 0; j < allPlayers.Count; j++)
                {
                    var data = allPlayers[j];
                    var oppositeUsed = data.DefaultOutfit.ColorId == localGradId && grads[data.PlayerId] == j;
                    var sameUsed = data.DefaultOutfit.ColorId == localColorId && grads[data.PlayerId]==j;

                    var noGrad = data.DefaultOutfit.ColorId == grads[data.PlayerId] && grads[data.PlayerId] == j;

                    if (noGrad)
                    {
                        __instance.AvailableColors.Remove(data.DefaultOutfit.ColorId);
                    }
                    
                    if (oppositeUsed||sameUsed)
                    {
                        __instance.AvailableColors.Remove(data.DefaultOutfit.ColorId);
                        __instance.AvailableColors.Remove(GradientColorManager.Instance.Gradients[data.PlayerId]);
                    }
                }
            }
            
            return false;
        }
        
        return true;
    }
}