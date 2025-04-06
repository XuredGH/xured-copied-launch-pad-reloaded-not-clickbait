using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Options.Roles.Impostor;
using LaunchpadReloaded.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Networking;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Helpers = LaunchpadReloaded.Utilities.Helpers;
using Random = System.Random;

namespace LaunchpadReloaded.Modifiers;

public class EatenModifier : TimedModifier
{
    public override string ModifierName => "Eaten";

    public override bool HideOnUi => false;
    public override bool AutoStart => true;
    public override float Duration => OptionGroupSingleton<DevourerOptions>.Instance.DevouredTime;

    public bool IsImpostor;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        IsImpostor = Player!.Data.Role.IsImpostor;

        Player!.cosmetics.SetName("DEVOURED");
        Player.cosmetics.SetNameMask(true);

        if (Player.cosmetics.gameObject.activeSelf)
        {
            Player.cosmetics.gameObject.SetActive(false);
        }

        if (!Player.AmOwner)
        {
            return;
        }
    }

    public override void OnActivate()
    {
        GradientManager.SetGradientEnabled(Player!, false);
        Player!.cosmetics.SetColor(15);

        if (Player.cosmetics.CurrentPet != null)
        {
            Player.cosmetics.CurrentPet.gameObject.SetActive(false);
        }

        Player.cosmetics.gameObject.SetActive(false);

        if (!Player.AmOwner)
        {
            return;
        }
    }

    public override void OnDeactivate()
    {
        GradientManager.SetGradientEnabled(Player!, true);
        Player!.cosmetics.SetColor((byte)Player.Data.DefaultOutfit.ColorId);

        if (Player.cosmetics.CurrentPet != null)
        {
            Player.cosmetics.CurrentPet.gameObject.SetActive(true);
        }

        Player.cosmetics.gameObject.SetActive(true);
        Player.SetName(Player.Data.PlayerName);

        if (!Player.AmOwner)
        {
            return;
        }

        if (IsImpostor)
        {
            return;
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }
}