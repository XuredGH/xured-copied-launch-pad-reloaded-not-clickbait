﻿using System;
using System.Collections;
using BepInEx.Configuration;
using LaunchpadReloaded.Components;
using Reactor.Utilities;

namespace LaunchpadReloaded.Features.Managers;


public static class GradientManager
{
    private static readonly ConfigEntry<int> GradientConfig =
        LaunchpadReloadedPlugin.Instance.Config.Bind("Gradient", "Secondary", 0, "Gradient ID");

    public static int LocalGradientId
    {
        get => GradientConfig.Value;
        set
        {
            try
            {
                GradientConfig.Value = value;
            }
            catch (Exception e)
            {
                Logger<LaunchpadReloadedPlugin>.Error(e.ToString());
            }
        }
    }

    public static void SetGradient(this PlayerControl pc, int colorId)
    {
        Coroutines.Start(WaitForDataCoroutine(pc, colorId));
    }
    
    public static void SetGradientEnabled(PlayerControl pc, bool enabled)
    {
        var gradData = pc.GetComponent<PlayerGradientData>();
        gradData.GradientEnabled = enabled;
        Coroutines.Start(WaitForDataCoroutine(pc, gradData.GradientColor));
    }

    private static IEnumerator WaitForDataCoroutine(PlayerControl pc, int colorId)
    {
        var gradData = pc.GetComponent<PlayerGradientData>();
        
        while (pc.Data is null || !gradData)
        {
            yield return null;
        }
        
        gradData.GradientColor = colorId;
        
        pc.SetColor(pc.Data.DefaultOutfit.ColorId);
    }

    public static bool TryGetColor(byte id, out byte color)
    {
        var data = GameData.Instance.GetPlayerById(id);
        if (data != null && data.Object)
        {
            var colorData = data.Object.GetComponent<PlayerGradientData>();
            if (colorData && colorData.GradientColor != 255)
            {
                color = (byte)colorData.GradientColor;
                return true;
            }
        }

        color = 0;
        return false;
    }
    public static bool TryGetEnabled(byte id, out bool enabled)
    {
        var data = GameData.Instance.GetPlayerById(id);
        if (data != null && data.Object)
        {
            var colorData = data.Object.GetComponent<PlayerGradientData>();
            if (colorData && colorData.GradientColor != 255)
            {
                enabled = colorData.GradientEnabled;
                return true;
            }
        }

        enabled = false;
        return false;
    }
}