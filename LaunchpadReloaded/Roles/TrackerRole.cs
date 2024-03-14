﻿using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Text;
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
        StringBuilder taskStringBuilder = Helpers.CreateForRole(this);

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
        foreach (ScannerComponent component in ScannerManager.Instance.Scanners)
        {
            if (component.Room != null) taskStringBuilder.AppendLine($"Scanner {component.Id} ({component.Room.RoomId})");
        }
        return taskStringBuilder;
    }

    public static CustomNumberOption PingTimer;
    public static CustomNumberOption ScannerCooldown;
    public static CustomNumberOption MaxScanners;
    public static CustomOptionGroup Group;
    public void CreateOptions()
    {
        PingTimer = new CustomNumberOption("Tracker Ping Timer",
            defaultValue: 7,
            range: new FloatRange(3, 30),
            increment: 1,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        MaxScanners = new CustomNumberOption("Max Scanners",
            defaultValue: 3,
            range: new FloatRange(1, 15),
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(TrackerRole));

        ScannerCooldown = new CustomNumberOption("Place Scanner Cooldown",
            defaultValue: 5,
            range: new FloatRange(1, 20),
            increment: 2,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Tracker</color>",
            numberOpt: [PingTimer, MaxScanners, ScannerCooldown],
            stringOpt: [],
            toggleOpt: [], role: typeof(TrackerRole));
    }
}
