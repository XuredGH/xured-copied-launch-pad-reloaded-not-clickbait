using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
public class CaptainRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Captain";
    
    public ushort RoleId => (ushort)LaunchpadRoles.Captain;
    
    public string RoleDescription => "Protect the crew with your abilities";
    
    public string RoleLongDescription => "Use your zoom ability to keep an eye on the crew\n And call meetings from any location!";
    
    public Color RoleColor => LaunchpadPalette.CaptainColor;
    
    public RoleTeamTypes Team => RoleTeamTypes.Crewmate;
    
    public override bool IsDead => false;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.ZoomButton;

    public static CustomNumberOption CaptainMeetingCooldown { get; private set; }
    public static CustomNumberOption CaptainMeetingCount { get; private set; }
    public static CustomNumberOption ZoomCooldown { get; private set; }
    public static CustomNumberOption ZoomDuration { get; private set; }
    public static CustomNumberOption ZoomDistance { get; private set; }
    public static CustomOptionGroup Group { get; private set; }

    public void CreateOptions()
    {
        CaptainMeetingCooldown = new CustomNumberOption("Meeting Cooldown",
            defaultValue: 45,
            0, 120,
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        CaptainMeetingCount = new CustomNumberOption("Meeting Uses",
            defaultValue: 3,
            1, 5,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(CaptainRole));

        ZoomCooldown = new CustomNumberOption("Zoom Cooldown",
            defaultValue: 30,
            5, 60,
            increment: 2,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        ZoomDuration = new CustomNumberOption("Zoom Duration",
            defaultValue: 5,
            5, 25,
            increment: 1,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(CaptainRole));

        ZoomDistance = new CustomNumberOption("Zoom Distance",
            defaultValue: 6,
            4, 15,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(CaptainRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Captain</color>",
            numberOpt: [CaptainMeetingCooldown, CaptainMeetingCount, ZoomCooldown, ZoomDuration, ZoomDistance],
            stringOpt: [],
            toggleOpt: [], role: typeof(CaptainRole));
    }
}