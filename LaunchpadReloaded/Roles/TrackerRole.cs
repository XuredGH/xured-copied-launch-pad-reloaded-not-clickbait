using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Text;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class TrackerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Tracker";
    public ushort RoleId => (ushort)LaunchpadRoles.Tracker;
    public string RoleDescription => "Track a player's movements.";
    public string RoleLongDescription => "Place a tracker on a player to track their movements on the map.\nPlace scanners to detect nearby player movement.\n";
    public Color RoleColor => LaunchpadPalette.TrackerColor;
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.TrackButton;
    
    [HideFromIl2Cpp]
    public StringBuilder SetTabText()
    {
        var taskStringBuilder = Helpers.CreateForRole(this);

        if (TrackingManager.Instance.TrackedPlayer)
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

        if (ScannerManager.Instance.scanners.Count > 0)
        {
            taskStringBuilder.AppendLine("<b>Created Scanners:</b>");
        }

        foreach (var component in ScannerManager.Instance.scanners)
        {
            if (component.room)
            {
                taskStringBuilder.AppendLine($"Scanner {component.id} ({component.room.RoomId})");
            }
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
            3, 30,
            increment: 1,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        MaxScanners = new CustomNumberOption("Max Scanners",
            defaultValue: 3,
            1, 15,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(TrackerRole));

        ScannerCooldown = new CustomNumberOption("Place Scanner Cooldown",
            defaultValue: 5,
            1, 20,
            increment: 2,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(TrackerRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Tracker</color>",
            numberOpt: [PingTimer, MaxScanners, ScannerCooldown],
            stringOpt: [],
            toggleOpt: [], role: typeof(TrackerRole));
    }
}
