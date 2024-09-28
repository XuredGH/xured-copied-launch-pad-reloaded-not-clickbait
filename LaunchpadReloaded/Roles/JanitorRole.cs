using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using System;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Roles;

[RegisterInIl2Cpp]
[RegisterCustomRole]
public class JanitorRole(IntPtr ptr) : ImpostorRole(ptr), ICustomRole
{
    public string RoleName => "Janitor";
    public string RoleDescription => "Drag bodies and hide them in vents";
    public string RoleLongDescription => "You can drag bodies and hide them in vents\nWhich will cause them to disappear unless the vent is used.";
    public Color RoleColor => LaunchpadPalette.JanitorColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
    public CustomRoleConfiguration Configuration => new()
    {
        Icon = LaunchpadAssets.DragButton,
    };

    public override bool CanUse(IUsable usable)
    {
        if (!GameManager.Instance.LogicUsables.CanUse(usable, Player))
        {
            return false;
        }
        var console = usable.TryCast<Console>();
        return !(console != null) || console.AllowImpostor && !PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>();
    }
}