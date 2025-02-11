using AmongUs.GameOptions;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using MiraAPI.Modifiers;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class RevivedModifier : BaseModifier
{
    public override string ModifierName => "Revived";
    private readonly int VisorColor = Shader.PropertyToID("_VisorColor");

    private PlayerTag RevivedTag = new PlayerTag()
    {
        Name = "RevivedTag",
        Text = "Revived",
        Color = LaunchpadPalette.MedicColor,
        IsLocallyVisible = (plr) => true,
    };

    public override void OnActivate()
    {
        Player.Revive();

        Player.RemainingEmergencies = GameManager.Instance.LogicOptions.GetNumEmergencyMeetings();
        RoleManager.Instance.SetRole(Player, RoleTypes.Crewmate);
        Player.Data.Role.SpawnTaskHeader(Player);
        Player.MyPhysics.SetBodyType(Player.BodyType);

        if (Player.AmOwner)
        {
            HudManager.Instance.MapButton.gameObject.SetActive(true);
            HudManager.Instance.ReportButton.gameObject.SetActive(true);
            HudManager.Instance.UseButton.gameObject.SetActive(true);
            Player.myTasks.RemoveAt(0);
        }

        var tagManager = Player.GetTagManager();

        if (tagManager != null)
        {
            var existingTag = tagManager.GetTagByName(RevivedTag.Name);
            if (existingTag.HasValue)
            {
                tagManager.RemoveTag(existingTag.Value);
            }

            tagManager.AddTag(RevivedTag);
        }
    }

    public override void OnDeactivate()
    {
        var tagManager = Player?.GetTagManager();

        if (tagManager != null)
        {
            tagManager.RemoveTag(RevivedTag);
        }
    }
    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void FixedUpdate()
    {
        Player!.cosmetics.visor.SetVisorColor(LaunchpadPalette.MedicColor);
        Player!.cosmetics.currentBodySprite.BodySprite.material.SetColor(VisorColor, LaunchpadPalette.MedicColor);

        if (MeetingHud.Instance)
        {
            var playerState = MeetingHud.Instance.playerStates.First(plr => plr.TargetPlayerId == Player!.PlayerId);
            if (playerState is null) return;

            playerState.PlayerIcon.cosmetics.visor.SetVisorColor(LaunchpadPalette.MedicColor);
            playerState.PlayerIcon.cosmetics.currentBodySprite.BodySprite.material.SetColor(VisorColor, LaunchpadPalette.MedicColor);
        }
    }
}