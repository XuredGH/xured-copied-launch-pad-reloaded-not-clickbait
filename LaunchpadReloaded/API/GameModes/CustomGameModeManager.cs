using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reactor.Utilities;

namespace LaunchpadReloaded.API.GameModes;

/// <summary>
/// Manages custom gamemodes
/// </summary>
public static class CustomGameModeManager
{
    /// <summary>
    /// List of registered gamemodes
    /// </summary>
    public static readonly List<CustomGameMode> GameModes = [];
    
    /// <summary>
    /// Current gamemode
    /// </summary>
    public static CustomGameMode ActiveMode;

    // TODO: MAKE AN ATTRIBUTE
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

    /// <summary>
    /// Sync gamemodes via RPC
    /// </summary>
    /// <param name="id">Gamemode ID</param>
    [MethodRpc((uint)LaunchpadRPC.SetGameMode)]
    public static void RpcSetGameMode(PlayerControl pc, int id)
    {
        SetGameMode(id);
    }

    /// <summary>
    /// Set current gamemode
    /// </summary>
    /// <param name="id">gamemode ID</param>
    public static void SetGameMode(int id)
    {
        ActiveMode = GameModes.Find(gameMode => gameMode.Id == id);
    }

    /// <summary>
    /// Register gamemode from type 
    /// </summary>
    /// <param name="gameModeType">Type of gamemode class, should inherit from <see cref="CustomGameMode"/></param>
    public static void RegisterGameMode(Type gameModeType)
    {
        if (!typeof(CustomGameMode).IsAssignableFrom(gameModeType))
        {
            Logger<LaunchpadReloadedPlugin>.Warning($"{gameModeType.Name} does not inherit CustomGameMode!");
            return;
        }

        var gameMode = (CustomGameMode)Activator.CreateInstance(gameModeType);
        
        if (GameModes.Any(x => x.Id == gameMode.Id))
        {
            Logger<LaunchpadReloadedPlugin>.Error($"ID for gamemode {gameMode.Name} already exists!");
            return;
        }
        
        GameModes.Add(gameMode);
    }
}