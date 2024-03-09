using AmongUs.Data;
using AmongUs.GameOptions;
using HarmonyLib;
using LaunchpadReloaded.API.Gamemodes;
using LaunchpadReloaded.Components;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static GameData;

namespace LaunchpadReloaded.Gamemodes;
public class BattleRoyale : CustomGamemode
{
    public override string Name => "Battle Royale";
    public override string Description => "Everyone can kill. <b><i>Last one standing wins.</b></i>";

    public override int Id => 1;

    public override void Begin()
    {

    }
    public override void HudUpdate(HudManager instance)
    {
        instance.AbilityButton.gameObject.SetActive(false);
        instance.UseButton.gameObject.SetActive(false);
        instance.ReportButton.gameObject.SetActive(false);
        instance.SabotageButton.gameObject.SetActive(false);
        instance.PetButton.gameObject.SetActive(false);
        instance.ImpostorVentButton.gameObject.SetActive(false);
        instance.KillButton.transform.position = instance.UseButton.transform.position;
    }

    public override void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = true;
/*        var alivePlayers = GameData.Instance.AllPlayers.ToArray()
            .Where(player => !player.Disconnected && !player.IsDead);
        if (alivePlayers.Count() == 1)
        {
            instance.Manager.RpcEndGame(GameOverReason.ImpostorByKill, false);
        }*/
    }

    public override void AssignRoles(out bool runOriginal, List<PlayerInfo> players, LogicRoleSelectionNormal instance)
    {
        runOriginal = false;
    }
}