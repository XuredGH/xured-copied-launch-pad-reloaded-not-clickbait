using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.GameOptions;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(IGameOptionsExtensions),nameof(IGameOptionsExtensions.ToHudString))]
public static class ToHudStringPatch
{
    public static bool ShowCustom = false;
    
    public static void AddOptions(StringBuilder sb,
        IEnumerable<CustomNumberOption> numberOptions, IEnumerable<CustomStringOption> stringOptions, IEnumerable<CustomToggleOption> toggleOptions)
    {
        foreach (var numberOption in numberOptions)
        {
            sb.AppendLine(numberOption.Title + ": " + numberOption.Value);
        }

        foreach (var toggleOption in toggleOptions)
        {
            sb.AppendLine(toggleOption.Title + ": " + toggleOption.Value);
        }

        foreach (var stringOption in stringOptions)
        {
            sb.AppendLine(stringOption.Title + ": " + stringOption.Value);
        }
    }

    public static void Postfix(IGameOptions __instance, ref string __result)
    {
        if(ShowCustom)
        {
            var sb = new StringBuilder("<size=125%><b>Launchpad Options:</b></size>\n");

            foreach(var group in CustomOptionsManager.CustomGroups)
            {
                if (group.Hidden()) continue;

                sb.AppendLine($"<size=110%><b>{group.Title}</b></size>");
                AddOptions(sb, group.CustomNumberOptions, group.CustomStringOptions, group.CustomToggleOptions);
                sb.Append("\n");
            }

            sb.AppendLine($"<size=110%><b>Other Options</b></size>");
            AddOptions(sb,
                CustomOptionsManager.CustomNumberOptions.Where(option => option.Group == null),
                CustomOptionsManager.CustomStringOptions.Where(option => option.Group == null),
                CustomOptionsManager.CustomToggleOptions.Where(option => option.Group == null));

            __result = sb.ToString() + "\n\nPress <b>Tab</b> to view Normal Options";
            return;
        }

        __result = "<size=125%><b>Normal Options:</b></size>\n" + __result + "\n\nPress <b>Tab</b> to view Launchpad Options";
    }
}