using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Utilities;

namespace LaunchpadReloaded.Patches.Options;

[HarmonyPatch(typeof(IGameOptionsExtensions), "ToHudString")]
public static class ToHudStringPatch
{
    /// <summary>
    /// Show custom Launchpad options or vanilla game options
    /// </summary>
    public static bool ShowCustom = false;

    /// <summary>
    /// Generic method to add options 
    /// </summary>
    public static void AddOptions(StringBuilder sb,
        IEnumerable<CustomNumberOption> numberOptions, IEnumerable<CustomStringOption> stringOptions, IEnumerable<CustomToggleOption> toggleOptions)
    {
        foreach (var numberOption in numberOptions)
        {
            sb.AppendLine(numberOption.Title + ": " + numberOption.Value + Helpers.GetSuffix(numberOption.SuffixType));
        }

        foreach (var toggleOption in toggleOptions)
        {
            sb.AppendLine(toggleOption.Title + ": " + (toggleOption.Value ? "On" : "Off"));
        }

        foreach (var stringOption in stringOptions)
        {
            sb.AppendLine(stringOption.Title + ": " + stringOption.Value);
        }
    }

    /// <summary>
    /// Update the HudOptions on the left of the screen if player is using Launchpad options
    /// </summary>
    public static void Postfix(IGameOptions __instance, ref string __result)
    {
        // Hide and Seek isn't compatible with Launchpad currently, so check if current mode is Hide and Seek and then return
        if (GameManager.Instance is null || GameManager.Instance.IsHideAndSeek())
        {
            return;
        }

        if (ShowCustom || !CustomGameModeManager.ActiveMode.CanAccessSettingsTab())
        {
            var sb = new StringBuilder("<size=180%><b>Launchpad Options:</b></size>\n<size=130%>");
            var groupsWithRoles = CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole != null);
            var groupsWithoutRoles = CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null);

            AddOptions(sb,
                CustomOptionsManager.CustomNumberOptions.Where(option => option.Group == null && !option.Hidden()),
                CustomOptionsManager.CustomStringOptions.Where(option => option.Group == null && !option.Hidden()),
                CustomOptionsManager.CustomToggleOptions.Where(option => option.Group == null && !option.Hidden()));

            foreach (var group in groupsWithoutRoles)
            {
                if (group.Hidden())
                {
                    continue;
                }

                sb.AppendLine($"\n<size=160%><b>{group.Title}</b></size>");
                AddOptions(sb, group.CustomNumberOptions, group.CustomStringOptions, group.CustomToggleOptions);
            }

            var customOptionGroups = groupsWithRoles as CustomOptionGroup[] ?? groupsWithRoles.ToArray();
            if (customOptionGroups.Any() && CustomGameModeManager.ActiveMode.CanAccessRolesTab())
            {
                sb.AppendLine("\n<size=160%><b>Roles</b></size>");

                foreach (var group in customOptionGroups)
                {
                    if (group.Hidden())
                    {
                        continue;
                    }

                    sb.AppendLine($"<size=140%><b>{group.Title}</b></size><size=120%>");
                    AddOptions(sb, group.CustomNumberOptions, group.CustomStringOptions, group.CustomToggleOptions);
                    sb.Append("</size>\n");
                }
            }

            var suffix = CustomGameModeManager.ActiveMode.CanAccessSettingsTab() ? "\nPress <b>Tab</b> to view Normal Options" :
                $"\n<b>You can not access Normal Options on {CustomGameModeManager.ActiveMode.Name} mode.</b>";

            __result = sb + suffix;
            return;
        }

        __result = "<size=160%><b>Normal Options:</b></size>\n<size=130%>" + __result + "\nPress <b>Tab</b> to view Launchpad Options</size>";
    }
}