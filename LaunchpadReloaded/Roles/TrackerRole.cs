using System;
using System.Text;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;
[RegisterInIl2Cpp]
public class TrackerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Tracker";

    public string RoleDescription => "Track a player's movements.";

    public string RoleLongDescription => "Place a tracker on a player to track their movements on the map.\nPlace scanners to detect nearby player movement.\n";

    public Color RoleColor => LaunchpadPalette.TrackerColor;

    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.TrackButton;

    public StringBuilder SetTabText()
    {
        var taskStringBuilder = Helpers.CreateForRole(this);

        if (TrackingManager.Instance.TrackedPlayer != null)
        {
            if (TrackingManager.Instance.TrackerDisconnected)
            {
                taskStringBuilder.AppendLine("<color=red>Tracker Disconnected.</color>");
            }
            else
            {
                taskStringBuilder.AppendLine($"Tracking: <b>{TrackingManager.Instance.TrackedPlayer.Data.Color.ToTextColor()}{TrackingManager.Instance.TrackedPlayer.Data.PlayerName}</b></color>");
                taskStringBuilder.AppendLine("Next ping in " + (int)TrackingManager.Instance.Timer + " seconds.\n");
            }
        }

        if (ScannerManager.Instance.Scanners.Count > 0) taskStringBuilder.AppendLine("<b>Created Scanners:</b>");
        foreach (var component in ScannerManager.Instance.Scanners)
        {
            if (component.Room != null) taskStringBuilder.AppendLine($"Scanner {component.Id} ({component.Room.RoomId})");
        }
        return taskStringBuilder;
    }
}
