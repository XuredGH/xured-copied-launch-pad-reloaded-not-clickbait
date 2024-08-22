using System;
using System.Collections.Generic;
using System.Linq;
using Cpp2IL.Core.Extensions;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using Reactor.Utilities.Attributes;
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

        closeButton.OnClick.AddListener((UnityAction)(()=>Close()));
        
        outsideButton.OnClick.AddListener((UnityAction)(()=>Close()));

        suspects = gameObject.transform.FindChild("Suspects").GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public void Open(LaunchpadPlayer deadPlayer)
    {
        // init body
        var timeSinceDeath = DateTime.Now.Subtract(deadPlayer.DeadData.DeathTime);
        deadPlayerInfo.text = timeSinceDeath.Minutes < 1 ? $"{deadPlayer.playerObject.Data.PlayerName}\n<size=70%>Died {timeSinceDeath.Seconds} seconds ago</size>" :
            $"{deadPlayer.playerObject.Data.PlayerName}\n<size=70%>Died {timeSinceDeath.Minutes} minutes ago</size>";

        deadPlayer.playerObject.SetPlayerMaterialColors(deadBodyIcon);

        if (LaunchpadPlayer.GetAllAlivePlayers().Count() < 4 || ModdedGroupSingleton<DetectiveOptions>.Instance.HideSuspects)
        {
            gameObject.transform.FindChild("Suspects").gameObject.SetActive(false);
            gameObject.transform.FindChild("SuspectsText").gameObject.SetActive(false);
            gameObject.transform.FindChild("SuspectsTitle").GetComponent<TextMeshPro>().text = "Suspects cannot be shown.";
            Begin(null);
            return;
        }

        var rand = new Random();
        var chosenRend = suspects[rand.Next(suspects.Count)];
        deadPlayer.DeadData.Killer.SetPlayerMaterialColors(chosenRend);

        var availableSprites = suspects;
        availableSprites.Remove(chosenRend);

        var sus = deadPlayer.DeadData.Suspects.Clone();

        foreach (var rend in availableSprites)
        {
            var randPlayer = sus[rand.Next(sus.Count)];
            randPlayer.SetPlayerMaterialColors(rend);
            sus.Remove(randPlayer);
        }

        Begin(null);
    }
}