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

public class HackedModifier : TimedModifier
{
    public override string ModifierName => "Hacked";

    public override bool HideOnUi => false;
    public override bool AutoStart => true;
    public override float Duration => OptionGroupSingleton<HackerOptions>.Instance.HackDuration;

    public bool IsImpostor;

    public bool DeActivating;

    private TextMeshPro _hackedText;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6),
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
        Player!.cosmetics.SetName(randomString);
        Player.cosmetics.SetNameMask(true);
        Player.cosmetics.gameObject.SetActive(false);

        if (_hackedText)
        {
            _hackedText.text = $"Find a node on the map to unhack!\nIf you don't unhack in time, <b>YOU WILL DIE.</b>\n<size=70%>{Math.Round(TimeRemaining, 0)} seconds remaining.</size>";
        }
    }

    public override void OnActivate()
    {
        IsImpostor = Player!.Data.Role.IsImpostor;

        GradientManager.SetGradientEnabled(Player, false);
        Player.cosmetics.SetColor(15);
        Player.cosmetics.gameObject.SetActive(false);

        if (!Player.AmOwner)
        {
            return;
        }
        foreach (var node in HackNodeComponent.AllNodes)
        {
            node.isActive = true;
        }

        if (IsImpostor)
        {
            return;
        }

        _hackedText = MiraAPI.Utilities.Helpers.CreateTextLabel("HackedText", HudManager.Instance.transform, AspectPosition.EdgeAlignments.Top, new Vector3(0, 1, 0));

        Coroutines.Start(HackEffect());
    }

    public override void OnDeactivate()
    {
        DeActivating = true;
        GradientManager.SetGradientEnabled(Player!, true);
        Player!.cosmetics.SetColor((byte)Player.Data.DefaultOutfit.ColorId);
        Player.cosmetics.gameObject.SetActive(true);
        Player.SetName(Player.Data.PlayerName);

        // remove hacked modifier from impostors when all crewmates fix hack
        if (!Player.AmOwner && PlayerControl.LocalPlayer.Data.Role.IsImpostor)
        {
            var shouldRemove = HackerUtilities.CountHackedPlayers(false) < 1;
            if (shouldRemove)
            {
                MiraAPI.Utilities.Extensions.RpcRemoveModifier<HackedModifier>(PlayerControl.LocalPlayer);
            }
        }

        if (!Player.AmOwner)
        {
            return;
        }

        foreach (var node in HackNodeComponent.AllNodes)
        {
            node.isActive = false;
        }

        if (IsImpostor)
        {
            return;
        }

        Coroutines.Stop(HackEffect());
        _hackedText.gameObject.DestroyImmediate();
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public static IEnumerator HackEffect()
    {
        var random = new Random();
        HudManager.Instance.TaskPanel.open = false;
        var originalPos = HudManager.Instance.ReportButton.transform.localPosition;
        var originalPos2 = HudManager.Instance.UseButton.transform.localPosition;
        var taskBar = HudManager.Instance.gameObject.GetComponentInChildren<ProgressTracker>();

        while (PlayerControl.LocalPlayer.Data.IsHacked())
        {
            HudManager.Instance.FullScreen.color = new Color32(0, 255, 0, 100);
            HudManager.Instance.FullScreen.gameObject.SetActive(!HudManager.Instance.FullScreen.gameObject.active);
            SoundManager.Instance.PlaySound(LaunchpadAssets.HackingSound.LoadAsset(), false, 0.6f);
            taskBar.curValue = random.NextSingle();
            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.TaskPanel.open = true;
                yield return new WaitForSeconds(0.1f);
                HudManager.Instance.TaskPanel.open = false;
            }

            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.ReportButton.transform.localPosition += new Vector3(-random.NextSingle() + 1, random.NextSingle() + 1);
                yield return new WaitForSeconds(0.2f);
                HudManager.Instance.ReportButton.transform.localPosition = originalPos;
            }

            if (random.Next(0, 2) == 1)
            {
                HudManager.Instance.UseButton.transform.localPosition += new Vector3(-random.NextSingle() + 1, random.NextSingle() + 1);
                yield return new WaitForSeconds(0.2f);
                HudManager.Instance.UseButton.transform.localPosition = originalPos2;
            }

            yield return new WaitForSeconds(0.6f);
        }

        if (HudManager.InstanceExists)
        {
            SoundManager.Instance.StopSound(LaunchpadAssets.HackingSound.LoadAsset());
            HudManager.Instance.FullScreen.gameObject.SetActive(false);
            HudManager.Instance.UseButton.transform.localPosition = originalPos2;
            HudManager.Instance.ReportButton.transform.localPosition = originalPos;
        }
    }

    public override void OnTimerComplete()
    {
        if (Player is not null && !IsImpostor)
        {
            Player.RpcCustomMurder(Player, true, false, false, false, false, true);
        }
    }
}