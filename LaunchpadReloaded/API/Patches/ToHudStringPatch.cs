using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadReloaded.API.Patches;

[HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
public static class ToHudStringPatch
{
    public static bool ShowCustom = false;

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

    public static void Postfix(IGameOptions __instance, ref string __result)
    {
        if (ShowCustom || !CustomGamemodeManager.ActiveMode.CanAccessSettingsTab())
        {
            var sb = new StringBuilder("<size=150%><b>Launchpad Options:</b></size>\n");
            var groupsWithRoles = CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole != null);
            var groupsWithoutRoles = CustomOptionsManager.CustomGroups.Where(group => group.AdvancedRole == null);

            foreach (var group in groupsWithoutRoles)
            {
                if (group.Hidden()) continue;

                sb.AppendLine($"<size=110%><b>{group.Title}</b></size>");
                AddOptions(sb, group.CustomNumberOptions, group.CustomStringOptions, group.CustomToggleOptions);
                sb.Append("\n");
            }

            if (groupsWithRoles.Count() > 0 && CustomGamemodeManager.ActiveMode.CanAccessRolesTab())
            {
                sb.AppendLine($"<size=120%><b>Roles</b></size>");

                foreach (var group in groupsWithRoles)
                {
                    if (group.Hidden()) continue;

                    sb.AppendLine($"<size=90%><b>{group.Title}</b></size><size=70%>");
                    AddOptions(sb, group.CustomNumberOptions, group.CustomStringOptions, group.CustomToggleOptions);
                    sb.Append("</size>\n");
                }
            }


            AddOptions(sb,
                CustomOptionsManager.CustomNumberOptions.Where(option => option.Group == null && !option.Hidden()),
                CustomOptionsManager.CustomStringOptions.Where(option => option.Group == null && !option.Hidden()),
                CustomOptionsManager.CustomToggleOptions.Where(option => option.Group == null && !option.Hidden()));

            string suffix = CustomGamemodeManager.ActiveMode.CanAccessSettingsTab() ? "\nPress <b>Tab</b> to view Normal Options" :
                $"\n<b>You can not access Normal Options on {CustomGamemodeManager.ActiveMode.Name} mode.</b>";

            __result = sb.ToString() + suffix;
            return;
        }

        __result = "<size=150%><b>Normal Options:</b></size>\n" + __result + "\nPress <b>Tab</b> to view Launchpad Options";
    }
}