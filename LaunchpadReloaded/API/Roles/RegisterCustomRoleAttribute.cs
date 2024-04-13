using System;
using System.Collections.Generic;
using System.Reflection;

namespace LaunchpadReloaded.API.Roles;


// thanks to Reactor's RegisterInIl2Cpp attribute for providing the template for this
[AttributeUsage(AttributeTargets.Class)]
public class RegisterCustomRoleAttribute : Attribute
{
    private static readonly HashSet<Assembly> RegisteredAssemblies = [];
    
    public static void Register(Assembly assembly)
    {
        if (!RegisteredAssemblies.Add(assembly)) return;

        foreach (var type in assembly.GetTypes())
        {
            var attribute = type.GetCustomAttribute<RegisterCustomRoleAttribute>();
            if (attribute != null)
            {
                CustomRoleManager.RegisterRole(type);
            }
        }
    }
}