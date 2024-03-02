using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch]
public class ButtonPatches
{
    public static IEnumerable<MethodBase> TargetMethods()
    {
        return AccessTools.GetTypesFromAssembly(typeof(ActionButton).Assembly)
            .Where(type => type.IsAssignableTo(typeof(ActionButton)))
            .SelectMany(type => type.GetMethods())
            .Where(method => method.Name=="DoClick");
    }

    public static bool Prefix()
    {
        return !Helpers.ShouldCancelClick();
    }

    
}