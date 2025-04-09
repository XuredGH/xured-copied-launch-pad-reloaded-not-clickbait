using HarmonyLib;
using Discord;
using MiraAPI;
using UnityEngine;

namespace LaunchpadReloaded
{   
    // credit to newmod (https://github.com/CallOfCreator/NewMod/blob/main/NewMod/DiscordStatus.cs) [i really cant get my own ideas huh]
    [HarmonyPatch(typeof(ActivityManager), nameof(ActivityManager.UpdateActivity))]
    public static class DiscordPlayStatusPatch
    {
        public static void Prefix([HarmonyArgument(0)] Activity activity)
        {
            if (activity == null) return;
            
            var isBeta = true;

            string details = $"Xuredpad v0.2.0" + (isBeta ? " (Beta)" : "(dev)");

            activity.Details = details;

            try 
            {
                if (activity.State == "In Menus")
                {
                    int maxPlayers = GameOptionsManager.Instance.currentNormalGameOptions.MaxPlayers;
                    var lobbyCode = GameStartManager.Instance.GameRoomNameCode.text;
                    var miraAPIVersion = MiraApiPlugin.Version;
                    var platform = Application.platform;

                   details += $" Players: {maxPlayers} | Lobby Code: {lobbyCode} | MiraAPI Version {miraAPIVersion} | Platform: {platform}";
                }

                else if (activity.State == "In Game")
                {
                    if (MeetingHud.Instance)
                    {
                        details +=  " | \nIn Meeting";
                    }
                }
                
                activity.Assets.SmallText = "Xuredpad Made With MiraAPI";
            }
            catch (System.Exception e)
            {
                return;
            }
        }
    }
}