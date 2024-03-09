using InnerNet;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using Rewired.UI.ControlMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.API.Gamemodes;
public static class CustomGamemodeManager
{
    public static List<CustomGamemode> Gamemodes = new List<CustomGamemode>();
    public static CustomGamemode ActiveMode = null;

    public static void RegisterAllGamemodes()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(CustomGamemode)) && !type.IsAbstract)
            {
                RegisterGamemode(type);
            }
        }
    }

    [MethodRpc((uint)LaunchpadRPC.SetGamemode)]
    public static void RpcSetGamemode(PlayerControl player, int id)
    {
        SetGamemode(id);
        LaunchpadGameOptions.Instance.Gamemodes.SetValue(id);
    }

    public static void SetGamemode(int id)
    {
        ActiveMode = CustomGamemodeManager.Gamemodes.Find(gamemode => gamemode.Id == id);
    }

    public static void RegisterGamemode(Type gamemodeType)
    {
        if (!typeof(CustomGamemode).IsAssignableFrom(gamemodeType))
        {
            return;
        }

        var gamemode = (CustomGamemode)Activator.CreateInstance(gamemodeType);
        Gamemodes.Add(gamemode);
    }
}