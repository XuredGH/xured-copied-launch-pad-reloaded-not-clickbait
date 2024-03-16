using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LaunchpadReloaded.API.GameModes;
public static class CustomGameModeManager
{
    public static readonly List<CustomGameMode> GameModes = [];
    public static CustomGameMode ActiveMode;

    public static void RegisterAllGameModes()
    {
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAssignableTo(typeof(CustomGameMode)) && !type.IsAbstract)
            {
                RegisterGameMode(type);
            }
        }
    }

    [MethodRpc((uint)LaunchpadRPC.SetGameMode)]
    public static void RpcSetGameMode(PlayerControl pc, int id)
    {
        SetGameMode(id);
    }

    public static void SetGameMode(int id)
    {
        ActiveMode = GameModes.Find(gameMode => gameMode.Id == id);
    }

    public static void RegisterGameMode(Type gameModeType)
    {
        if (!typeof(CustomGameMode).IsAssignableFrom(gameModeType))
        {
            return;
        }

        var gameMode = (CustomGameMode)Activator.CreateInstance(gameModeType);
        GameModes.Add(gameMode);
    }
}