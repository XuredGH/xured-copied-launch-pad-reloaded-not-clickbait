using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(IGameOptionsExtensions),nameof(IGameOptionsExtensions.ToHudString))]
public static class ToHudStringPatch
{
    public static bool ShowCustom = false;
    
    public static void Postfix(IGameOptions __instance, ref string __result)
    {
        var sb = new StringBuilder("\n<size=125%><color=orange><b>Custom Game Options:</color></b></size>\n");

        foreach (var numberOption in CustomOptionsManager.CustomNumberOptions)
        {
            sb.AppendLine(numberOption.Title+": "+numberOption.Value);
        }

        foreach (var toggleOption in CustomOptionsManager.CustomToggleOptions)
        {
            sb.AppendLine(toggleOption.Title+": "+toggleOption.Value);
        }
    

        __result = "<size=125%><b>Normal Game Options:</b></size>\n" + __result + sb;
    }
}