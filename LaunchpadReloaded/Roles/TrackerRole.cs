using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Roles;
[RegisterInIl2Cpp]
public class TrackerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Tracker";

    public string RoleDescription => "Track a player's movements.";

    public string RoleLongDescription => "Place a tracker on a player and every 15 seconds it will place a marker where the players current position is on the map, which will help you to track their movements.";

    public Color RoleColor => new Color32(67, 166, 198, 255);

    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;

    public override void AppendTaskHint(Il2CppSystem.Text.StringBuilder taskStringBuilder)
    {
        if(TrackingManager.TrackedPlayer != null)
        {
            if(TrackingManager.TrackerDisconnected)
            {
                taskStringBuilder.AppendLine("\n<color=red>Tracker Disconnected.</color>");
                return;
            }

            taskStringBuilder.AppendLine("\nTracking: " + TrackingManager.TrackedPlayer.Data.PlayerName);
            taskStringBuilder.AppendLine("Next ping in " + (int) TrackingManager.Timer + " seconds.");
            return;
        }

        taskStringBuilder.AppendLine("\nPlace a tracker on a player to track their movements.");
    }
}
