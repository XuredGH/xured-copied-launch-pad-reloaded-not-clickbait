using AmongUs.GameOptions;
using Il2CppSystem;
using LaunchpadReloaded.Features;
using MiraAPI.Modifiers;
using UnityEngine;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class RevivedModifier : BaseModifier
{
    public override string ModifierName => "Revived";

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
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void FixedUpdate()
    {
        Player!.cosmetics.SetOutline(true, new Nullable<Color>(LaunchpadPalette.MedicColor));
    }
}