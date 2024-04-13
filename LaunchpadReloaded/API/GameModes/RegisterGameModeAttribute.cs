using System;
using System.Collections.Generic;
using System.Reflection;
using LaunchpadReloaded.API.Hud;

namespace LaunchpadReloaded.API.GameModes;

public class RegisterGameModeAttribute : Attribute
{
    private static readonly HashSet<Assembly> RegisteredAssemblies = [];
    
    public static void Register(Assembly assembly)
    {
        if (!RegisteredAssemblies.Add(assembly)) return;

        foreach (var type in assembly.GetTypes())
        {
            var attribute = type.GetCustomAttribute<RegisterGameModeAttribute>();
            if (attribute != null)
            {
                CustomGameModeManager.RegisterGameMode(type);
            }
        }
    }
}