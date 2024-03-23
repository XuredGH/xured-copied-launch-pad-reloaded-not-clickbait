using System.Collections;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace LaunchpadReloaded.Features.Managers;


public static class GradientManager
{
    public static int LocalGradientId { get; set; }


    [MethodRpc((uint)LaunchpadRPC.SyncGradient)]
    public static void RpcSetGradient(PlayerControl pc, int colorId)
    {
        pc.GetComponent<PlayerGradientData>().GradientColor = colorId;
        Coroutines.Start(WaitForDataCoroutine(pc));
    }

    [MethodRpc((uint)LaunchpadRPC.SetGradientEnabled)]
    public static void RpcSetGradientEnabled(PlayerControl pc, bool enabled)
    {
        pc.GetComponent<PlayerGradientData>().GradientEnabled = enabled;
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