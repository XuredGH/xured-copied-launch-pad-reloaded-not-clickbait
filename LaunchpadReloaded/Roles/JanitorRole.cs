using LaunchpadReloaded.API.GameOptions;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using Il2CppInterop.Runtime.Attributes;
using LaunchpadReloaded.Features.Managers;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public ushort RoleId => (ushort)LaunchpadRoles.Janitor;
    public string RoleDescription => "Drag bodies and hide them in vents";
    public string RoleLongDescription => "You can drag bodies and hide them in vents\nWhich will cause them to disappear unless the vent is used.";
    public Color RoleColor => LaunchpadPalette.JanitorColor;
    public RoleTeamTypes Team => RoleTeamTypes.Impostor;
    public override bool IsDead => false;
    public bool TargetsBodies => true;
    
    [HideFromIl2Cpp]
    public LoadableAsset<Sprite> Icon => LaunchpadAssets.DragButton;

    public static CustomNumberOption HideCooldown;
    public static CustomNumberOption HideUses;
    public static CustomToggleOption CleanInsteadOfHide;
    public static CustomOptionGroup Group;


    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }
        var console = usable.TryCast<Console>();
        return !(console != null) || console.AllowImpostor && !DragManager.Instance.DraggingPlayers.ContainsKey(PlayerControl.LocalPlayer.PlayerId);
    }

    public void CreateOptions()
    {
        HideCooldown = new CustomNumberOption("Hide Bodies Cooldown",
            defaultValue: 5,
            0, 120,
            increment: 5,
            suffixType: NumberSuffixes.Seconds,
            role: typeof(JanitorRole));

        HideUses = new CustomNumberOption("Hide Bodies Uses",
            defaultValue: 3,
            1, 10,
            increment: 1,
            suffixType: NumberSuffixes.None,
            role: typeof(JanitorRole));

        Group = new CustomOptionGroup($"{RoleColor.ToTextColor()}Janitor</color>",
            numberOpt: [HideCooldown, HideUses],
            stringOpt: [],
            toggleOpt: [], role: typeof(JanitorRole));
    }
}