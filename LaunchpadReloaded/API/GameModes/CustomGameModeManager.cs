using System;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Features;
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
    public static readonly Dictionary<int, CustomGameMode> GameModes = [];

    public static bool IsDefault()
    {
        return ActiveMode.Id == (int)LaunchpadGamemodes.Default;
    }
    
    
    /// <summary>
    /// Current gamemode
    /// </summary>
    public static CustomGameMode ActiveMode
    {
        get
        {
            if (GameManager.Instance.IsHideAndSeek())
            {
                return GameModes[(int)LaunchpadGamemodes.Default];
            }

            return GameModes.TryGetValue(LaunchpadGameOptions.Instance.GameModes.IndexValue, out var gameMode) ? gameMode : GameModes[(int)LaunchpadGamemodes.Default];
        }
        private set => LaunchpadGameOptions.Instance.GameModes.SetValue(value.Id);
    }


    /// <summary>
    /// Set current gamemode
    /// </summary>
    /// <param name="id">gamemode ID</param>
    public static void SetGameMode(int id)
    {
        if (GameModes.TryGetValue(id, out var gameMode))
        {
            ActiveMode = gameMode;
            return;
        }
        
        Logger<LaunchpadReloadedPlugin>.Error($"No gamemode with id {id} found!");
    }

    /// <summary>
    /// Register gamemode from type 
    /// </summary>
    /// <param name="gameModeType">Type of gamemode class, should inherit from <see cref="CustomGameMode"/></param>
    public static void RegisterGameMode(Type gameModeType)
    {
        if (!typeof(CustomGameMode).IsAssignableFrom(gameModeType))
        {
            Logger<LaunchpadReloadedPlugin>.Warning($"{gameModeType?.Name} does not inherit CustomGameMode!");
            return;
        }

        var gameMode = (CustomGameMode)Activator.CreateInstance(gameModeType);

        if (GameModes.Any(x => x.Key == gameMode?.Id))
        {
            Logger<LaunchpadReloadedPlugin>.Error($"ID for gamemode {gameMode?.Name} already exists!");
            return;
        }

        if (gameMode == null)
        {
            Logger<LaunchpadReloadedPlugin>.Error($"GAMEMODE WITH TYPE {gameModeType.Name} IS NULL");
            return;
        }
        
        GameModes.Add(gameMode.Id, gameMode);
    }
}