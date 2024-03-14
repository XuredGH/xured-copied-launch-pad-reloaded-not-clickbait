using System;
using System.Collections.Generic;
using System.Reflection;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;

namespace LaunchpadReloaded.API.Gamemodes;
public static class CustomGamemodeManager
{
    public static List<CustomGamemode> Gamemodes = new List<CustomGamemode>();
    public static CustomGamemode ActiveMode;

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
    }

    public static void SetGamemode(int id)
    {
        ActiveMode = Gamemodes.Find(gamemode => gamemode.Id == id);
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