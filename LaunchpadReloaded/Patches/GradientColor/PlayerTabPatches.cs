﻿using AmongUs.Data;
using HarmonyLib;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.Patches.GradientColor;

[HarmonyPatch(typeof(PlayerTab))]
public static class PlayerTabPatches
{
    public static bool SelectGradient;
    private static ColorChip _switchButton;
    private static TextMeshPro _titleText;

    private static void SwitchSelector(PlayerTab instance)
    {
        SelectGradient = !SelectGradient;
        _titleText.text = SelectGradient ? "Secondary Color: " : "Main Color: ";
        instance.currentColor = SelectGradient ? GradientColorManager.Instance.LocalGradientId : DataManager.Player.Customization.Color;
    }
    
    
    [HarmonyPostfix]
    [HarmonyPatch("OnEnable")]
    public static void OnEnablePostfix(PlayerTab __instance)
    {
        if (!_switchButton)
        {
            __instance.transform.FindChild("Text").GetComponent<TextMeshPro>().text = "Main Color: ";
            _switchButton = Object.Instantiate(__instance.ColorTabPrefab, __instance.ColorTabArea);
        
            var spriteRenderer = _switchButton.GetComponent<SpriteRenderer>();
            var sprite = spriteRenderer.sprite = LaunchpadReloadedPlugin.BlankButton;
        
            _switchButton.GetComponent<BoxCollider2D>().size = sprite.rect.size/sprite.pixelsPerUnit;
            _switchButton.transform.localScale = new Vector3(1, 1, 1);
            _switchButton.transform.localPosition = new Vector3(2, 1.5f, -2);
            
            var buttonText = Object.Instantiate(__instance.transform.Find("Text").gameObject,_switchButton.transform);
            buttonText.transform.localPosition = new Vector3(0, 0, 0);
            buttonText.GetComponent<TextTranslatorTMP>().Destroy();
            
            _titleText = buttonText.GetComponent<TextMeshPro>();
            _titleText.alignment = TextAlignmentOptions.Center;
            _titleText.text = SelectGradient ? "Main Color" : "Secondary\nColor";
            _titleText.fontSize = _titleText.fontSizeMax = 4;
        
            _switchButton.Button.OnClick.RemoveAllListeners();
            _switchButton.Button.OnMouseOut.RemoveAllListeners();
            _switchButton.Button.OnMouseOver.RemoveAllListeners();
            _switchButton.Button.OnClick.AddListener((UnityAction)(() => { SwitchSelector(__instance); }));
        }

        foreach (var colorChip in __instance.ColorChips)
        {
            colorChip.Button.OnMouseOut.RemoveAllListeners();
            colorChip.Button.OnMouseOut.AddListener((UnityAction)(() =>
            {
                __instance.SelectColor(SelectGradient ? GradientColorManager.Instance.LocalGradientId : DataManager.Player.Customization.Color);
            }));
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.ClickEquip))]
    public static bool ClickPrefix(PlayerTab __instance)
    {
        if (SelectGradient && __instance.AvailableColors.Remove(__instance.currentColor))
        {
            GradientColorManager.Instance.LocalGradientId = __instance.currentColor;
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
        if (SelectGradient)
        {
            __instance.UpdateAvailableColors();
            __instance.currentColor = colorId;
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
        if (SelectGradient)
        {
            __result = GradientColorManager.Instance.LocalGradientId;
            return false;
        }

        return true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerTab.Update))]
    public static void UpdatePostfix(PlayerTab __instance)
    {
        if(_titleText) _titleText.text = SelectGradient ? "Secondary Color: " : "Main Color: ";
        if (SelectGradient)
        {
            __instance.currentColorIsEquipped = __instance.currentColor == GradientColorManager.Instance.LocalGradientId;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerTab.UpdateAvailableColors))]
    public static bool UpdateColorsPrefix(PlayerTab __instance)
    {
        for (var i = 0; i < Palette.PlayerColors.Length; i++)
        {
            __instance.AvailableColors.Add(i);
        }
        // VERY BUGGY NEEDS TO BE REWRITTEN
        if (GameData.Instance)
        {
            var allPlayers = GameData.Instance.AllPlayers.ToArray();
            var grads = GradientColorManager.Instance.Gradients;
            var localGradId = GradientColorManager.Instance.LocalGradientId;
            var localColorId = PlayerControl.LocalPlayer.Data.DefaultOutfit.ColorId;
            foreach (var data in allPlayers)
            {
                if (SelectGradient)
                {
                    var isSame = data.DefaultOutfit.ColorId == localColorId;
                    var isOpposite = grads[data.PlayerId] == localColorId;
                    if (isSame)
                    {
                        __instance.AvailableColors.Remove(grads[data.PlayerId]);
                    }

                    if (isOpposite)
                    {
                        __instance.AvailableColors.Remove(data.DefaultOutfit.ColorId);
                    }
                }
                else
                {
                    var isSame = grads[data.PlayerId] == localGradId;
                    var isOpposite = data.DefaultOutfit.ColorId == localGradId;
                    if (isSame)
                    {
                        __instance.AvailableColors.Remove(data.DefaultOutfit.ColorId);
                    }

                    if (isOpposite)
                    {
                        __instance.AvailableColors.Remove(grads[data.PlayerId]);
                    }
                    
                }
            }
        }
        
        return false;
    }
}