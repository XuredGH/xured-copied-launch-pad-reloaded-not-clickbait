using LaunchpadReloaded.Features;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Crewmate;

public class GamblerRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Gambler";
    public string RoleDescription => "Guess a player's role to reveal it!";
    public string RoleLongDescription => "Guess a player's role to reveal it!\nHowever, if you get it incorrect, you will die.";
    public Color RoleColor => LaunchpadPalette.GamblerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.GambleButton,
        OptionsScreenshot = LaunchpadAssets.DetectiveBanner,
    };

    public bool CanLocalPlayerSeeRole(PlayerControl player)
    {
        if (PlayerControl.LocalPlayer.HasModifier<HackedModifier>()) return false;
        if (player.HasModifier<RevealedModifier>()) return true;

        return PlayerControl.LocalPlayer.Data.IsDead;
    }
}
