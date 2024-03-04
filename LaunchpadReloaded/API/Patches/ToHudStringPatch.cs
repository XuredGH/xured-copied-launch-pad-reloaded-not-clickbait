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
        var baseText = "";
        if (ShowCustom)
        {
            baseText = "<size=125%><b>Normal Game Options:</b></size><color=\"green\">Press TAB to see vanilla settings\n";

            var sb = new StringBuilder("<size=125%><b>Normal Game Options:</b></size>");

            foreach (var numberOption in CustomGameOptionsManager.CustomNumberOptions)
            {
                sb.AppendLine(numberOption.Title+": "+numberOption.Value);
            }

            foreach (var toggleOption in CustomGameOptionsManager.CustomToggleOptions)
            {
                sb.AppendLine(toggleOption.Title+": "+toggleOption.Value);
            }

        }
        else
        {
            baseText = "<size=125%><b>Normal Game Options:</b></size>\n<color=\"green\">Press TAB to custom settings\n";
        }

        __result = baseText + __result;
    }
}