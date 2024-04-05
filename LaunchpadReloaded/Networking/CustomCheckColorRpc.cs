using System.Linq;
using Hazel;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Reactor.Utilities;

namespace LaunchpadReloaded.Networking;

[RegisterCustomRpc((uint)LaunchpadRpc.CustomCheckColor)]
public class CustomCheckColorRpc(LaunchpadReloadedPlugin plugin, uint id)
    : PlayerCustomRpc<LaunchpadReloadedPlugin, CustomCheckColorRpc.Data>(plugin, id)
{
    public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

    public struct Data(byte color, byte gradient)
    {
        public readonly byte Color = color;
        public readonly byte Gradient = gradient;
    }
    
    public override void Write(MessageWriter writer, Data data)
    {
        writer.Write(data.Color);
        writer.Write(data.Gradient);
    }

    public override Data Read(MessageReader reader)
    {
        return new Data(reader.ReadByte(), reader.ReadByte());
    }
    
    public override void Handle(PlayerControl source, Data data)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return;
        }
        
        var bodyColor = data.Color;
        var gradColor = data.Gradient;

        if (!LaunchpadGameOptions.Instance.UniqueColors.Value)
        {
            GameData.Instance.CustomSetColor(source, bodyColor, gradColor);
            return;
        }
        
        var allPlayers = GameData.Instance.AllPlayers.ToArray();
        
        var num = 0;
        while (num++ < 127 && (gradColor >= Palette.PlayerColors.Length || allPlayers.Any(p =>
                   !p.Disconnected && p.PlayerId != source.PlayerId && !VerifyColor(bodyColor, gradColor))))
        {
            gradColor = (byte) ((gradColor + 1) % Palette.PlayerColors.Length);
        }
        
        num = 0;
        while (num++ < 127 && (bodyColor >= Palette.PlayerColors.Length || allPlayers.Any(p =>
                   !p.Disconnected && p.PlayerId != source.PlayerId && !VerifyColor(bodyColor, gradColor))))
        {
            bodyColor = (byte) ((bodyColor + 1) % Palette.PlayerColors.Length);
        }
        
        
        GameData.Instance.CustomSetColor(source, bodyColor, gradColor);
    }

    private static bool VerifyColor(byte requestedColor, byte requestedGradient)
    {
        var allPlayers = GameData.Instance.AllPlayers;
        
        foreach (var data in allPlayers)
        {
            if (!GradientManager.TryGetColor(data.PlayerId, out var gradColor))
            {
                Logger<LaunchpadReloadedPlugin>.Error($"Error getting gradient for player {data.PlayerName}");
                if (requestedColor == data.DefaultOutfit.ColorId)
                {
                    return false;
                }
            }

            if (requestedColor == data.DefaultOutfit.ColorId && requestedGradient == gradColor)
            {
                return false;
            }
            
            if (requestedColor == gradColor && requestedGradient == data.DefaultOutfit.ColorId)
            {
                return false;
            }
        }

        return true;
    }
}