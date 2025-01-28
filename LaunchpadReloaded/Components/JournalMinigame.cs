using Cpp2IL.Core.Extensions;
using LaunchpadReloaded.Modifiers;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class JournalMinigame(nint ptr) : Minigame(ptr)
{
    public TextMeshPro deadPlayerInfo;
    public PassiveButton closeButton;
    public PassiveButton outsideButton;
    public SpriteRenderer deadBodyIcon;
    public List<SpriteRenderer> suspects;

    private void Awake()
    {
        outsideButton = transform.FindChild("Background/OutsideCloseButton").GetComponent<PassiveButton>();
        closeButton = transform.FindChild("CloseButton").GetComponent<PassiveButton>();
        deadPlayerInfo = transform.FindChild("BodyInfo/DeadPlayerInfo").GetComponent<TextMeshPro>();
        deadBodyIcon = transform.FindChild("BodyInfo/Icon").GetComponent<SpriteRenderer>();

        closeButton.OnClick.AddListener((UnityAction)(() => Close()));

        outsideButton.OnClick.AddListener((UnityAction)(() => Close()));

        suspects = gameObject.transform.FindChild("Suspects").GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public void Open(PlayerControl deadPlayer)
    {
        var deathData = deadPlayer.GetModifier<DeathData>();

        if (deathData == null)
        {
            return;
        }

        var timeSinceDeath = DateTime.UtcNow.Subtract(deathData.DeathTime);
        deadPlayerInfo.text = timeSinceDeath.Minutes < 1 ? $"{deadPlayer.Data.PlayerName}\n<size=70%>Died {timeSinceDeath.Seconds} seconds ago</size>" :
            $"{deadPlayer.Data.PlayerName}\n<size=70%>Died {timeSinceDeath.Minutes} minutes ago</size>";

        deadPlayer.SetPlayerMaterialColors(deadBodyIcon);

        if (GameManager.Instance.LogicFlow.GetPlayerCounts().Item1 < 4 || OptionGroupSingleton<DetectiveOptions>.Instance.HideSuspects)
        {
            gameObject.transform.FindChild("Suspects").gameObject.SetActive(false);
            gameObject.transform.FindChild("SuspectsText").gameObject.SetActive(false);
            gameObject.transform.FindChild("SuspectsTitle").GetComponent<TextMeshPro>().text = "Suspects cannot be shown.";
            Begin(null);
            return;
        }

        var rand = new Random();
        var chosenRend = suspects[rand.Next(suspects.Count)];
        deathData.Killer.SetPlayerMaterialColors(chosenRend);

        var availableSprites = suspects;
        availableSprites.Remove(chosenRend);

        var sus = deathData.Suspects.Clone();

        foreach (var rend in availableSprites)
        {
            var randPlayer = sus[rand.Next(sus.Count)];
            randPlayer.SetPlayerMaterialColors(rend);
            sus.Remove(randPlayer);
        }

        Begin(null);
    }
}