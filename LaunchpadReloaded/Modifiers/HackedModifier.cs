using System.Collections;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Utilities;
using MiraAPI.Modifiers;
using Reactor.Utilities;
using UnityEngine;
using Random = System.Random;

namespace LaunchpadReloaded.Modifiers;

[RegisterModifier]
public class HackedModifier : BaseModifier
{
    public override string ModifierName => "Hacked";

    public bool IsImpostor;
    
    public override void FixedUpdate()
    {
        var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6),
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
        Player!.cosmetics.SetName(randomString);
        Player.cosmetics.SetNameMask(true);
        Player.cosmetics.gameObject.SetActive(false);
    }

    public override void OnActivate()
    {
        IsImpostor = Player.Data.Role.IsImpostor;

        GradientManager.SetGradientEnabled(Player, false);
        Player.cosmetics.SetColor(15);
        Player.cosmetics.gameObject.SetActive(false);

        if (!Player.AmOwner || IsImpostor)
        {
            return;
        }

        Coroutines.Start(HackEffect());   
        foreach (var node in HackNodeComponent.AllNodes)
        {
            node.isActive = true;
        }
    }

    public override void OnDeactivate()
    {
        GradientManager.SetGradientEnabled(Player, true);
        Player.cosmetics.SetColor((byte)Player.Data.DefaultOutfit.ColorId);
        Player.cosmetics.gameObject.SetActive(true);
        Player.SetName(Player.Data.PlayerName);

        // remove hacked modifier from impostors when all crewmates fix hack
        if (!Player.AmOwner && PlayerControl.LocalPlayer.Data.Role.IsImpostor)
        {
            var shouldRemove = HackerUtilities.CountHackedPlayers(false) <= 1; // account for current player being unhacked
            if (shouldRemove)
            {
                MiraAPI.Utilities.Extensions.RpcRemoveModifier<HackedModifier>(PlayerControl.LocalPlayer);
            }
        }

        if (!Player.AmOwner || IsImpostor)
        {
            return;
        }

        Coroutines.Stop(HackEffect());
        foreach (var node in HackNodeComponent.AllNodes)
        {
            node.isActive = false;
        }
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
}