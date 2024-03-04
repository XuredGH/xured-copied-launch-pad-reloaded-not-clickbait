using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Roles;
[RegisterInIl2Cpp]
public class TrackerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Tracker";

    public string RoleDescription => "Track a player's movements.";

    public string RoleLongDescription => "Place a tracker on a player to track their movements.\nPlace scanners to detect player movement.\n";

    public Color RoleColor => new Color32(67, 166, 198, 255);

    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;

    public List<ScannerComponent> PlacedScanners = new List<ScannerComponent>();

    public StringBuilder SetTabText()
    {
        StringBuilder taskStringBuilder = Helpers.CreateForRole(this);

        if (TrackingManager.TrackedPlayer != null)
        {
            if (TrackingManager.TrackerDisconnected)
            {
                taskStringBuilder.AppendLine("<color=red>Tracker Disconnected.</color>");
            }
            else
            {
                taskStringBuilder.AppendLine($"Tracking: <b>{TrackingManager.TrackedPlayer.Data.Color.ToTextColor()}{TrackingManager.TrackedPlayer.Data.PlayerName}</b></color>");
                taskStringBuilder.AppendLine("Next ping in " + (int)TrackingManager.Timer + " seconds.\n");
            }
        }

        foreach (ScannerComponent component in PlacedScanners)
        {
            taskStringBuilder.AppendLine($"<b>Scanner {component.Id}: </b>");
            if (component.PlayersInProximity.Count == 0) taskStringBuilder.AppendLine("No players in proximity.\n");
            foreach (PlayerControl player in component.PlayersInProximity)
            {
                taskStringBuilder.AppendLine($"{player.Data.Color.ToTextColor()}{player.Data.PlayerName}</color>");
            }
        }

        return taskStringBuilder;
    }
}
