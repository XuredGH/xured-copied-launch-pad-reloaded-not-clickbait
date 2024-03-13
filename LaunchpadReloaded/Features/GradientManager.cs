using System.Collections;
using System.Collections.Generic;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Features;


public static class GradientManager
{
    public static int LocalGradientId { get; set; }
    

    [MethodRpc((uint)LaunchpadRPC.SyncGradient)]
    public static void RpcSetGradient(PlayerControl pc, int colorId)
    {
        pc.GetComponent<PlayerGradientData>().gradientColor = colorId;
        Debug.LogError(pc.AmOwner);
        Coroutines.Start(WaitForDataCoroutine(pc));
    }
    
    private static IEnumerator WaitForDataCoroutine(PlayerControl pc)
    {
        while (pc.Data is null)
        {
            yield return null;
        }
        pc.SetColor(pc.Data.DefaultOutfit.ColorId);
    }

    public static bool TryGetColor(byte id, out byte color)
    {
        var data = GameData.Instance.GetPlayerById(id);
        if (data != null && data.Object)
        {
            var colorData = data.Object.GetComponent<PlayerGradientData>();
            if (colorData && colorData.gradientColor != 255)
            {
                color = (byte)colorData.gradientColor;
                return true;
            }
        }
        
        color = 0;
        Debug.LogError($"No player data for {id}");
        return false;
    }
}