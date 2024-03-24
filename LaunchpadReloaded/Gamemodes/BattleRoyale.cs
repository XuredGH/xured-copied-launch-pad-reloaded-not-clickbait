using System.Collections;
using System.Linq;
using AmongUs.GameOptions;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Gamemodes;
public class BattleRoyale : CustomGameMode
{
    public override string Name => "Battle Royale";
    public override string Description => "Everyone can kill.\n<b><i>Last one standing wins.</b></i>";

    public override int Id => (int)LaunchpadGamemodes.BattleRoyale;
    public TextMeshPro PlayerCount;
    public TextMeshPro DeathNotif;
    public override void Initialize()
    {
        var tasks = PlayerControl.LocalPlayer.myTasks;
        tasks.Clear();

        var random = ShipStatus.Instance.DummyLocations.Random();

        foreach (var player in GameData.Instance.AllPlayers)
            player.Object.cosmetics.TogglePet(false);

        PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(random.position);
        if (LaunchpadGameOptions.Instance.SeekerCharacter.Value)
        {
            GenericRPC.RpcSetBodyType(PlayerControl.LocalPlayer, 6);
        }
    }

    public IEnumerator DeathNotification(PlayerControl player)
    {
        var text = $"{player.Data.Color.ToTextColor()}{player.Data.PlayerName}</color> has <b>{Palette.ImpostorRed.ToTextColor()}DIED.</b></color>";
        DeathNotif.text = text;
        DeathNotif.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        DeathNotif.gameObject.SetActive(false);
    }
    public override void OnDeath(PlayerControl player)
    {
        var alivePlayers = GameData.Instance.AllPlayers.ToArray().Count(info => !info.Disconnected && !info.IsDead);
        player.roleAssigned = false;
        player.RpcSetRole(RoleTypes.CrewmateGhost);

        if (alivePlayers == 1)
        {
            return;
        }

        Coroutines.Start(DeathNotification(player));
    }
    public override void HudStart(HudManager instance)
    {
        DeathNotif = Helpers.CreateTextLabel("BR_DeathNotif", instance.transform, AspectPosition.EdgeAlignments.Bottom, new Vector3(0, 1f, 0));
        PlayerCount = Helpers.CreateTextLabel("BR_PlayerCounter", instance.transform, AspectPosition.EdgeAlignments.Top, new Vector3(0, 0.25f, 0));

        DeathNotif.text = "Death";
        DeathNotif.gameObject.SetActive(false);
    }

    public override bool CanAccessRolesTab() => false;

    public override void HudUpdate(HudManager instance)
    {
        if (PlayerCount)
        {
            var alivePlayers = GameData.Instance.AllPlayers.ToArray().Where(player => !player.Disconnected && !player.IsDead);
            PlayerCount.text = $"<size=75%>{Palette.ImpostorRed.ToTextColor()}Battle Royale</size></color>\n{alivePlayers.Count()} Players Remaining.";
        }

        instance.TaskStuff.gameObject.SetActive(false);
        instance.UseButton.gameObject.SetActive(true);
        instance.ReportButton.gameObject.SetActive(false);
        instance.SabotageButton.gameObject.SetActive(false);
        instance.PetButton.gameObject.SetActive(false);
        instance.ImpostorVentButton.gameObject.SetActive(false);
    }

    /*    public override List<GameData.PlayerInfo> CalculateWinners()
        {
            var alivePlayers = GameData.Instance.AllPlayers.ToArray().Where(player => !player.Disconnected && !player.IsDead).ToList();
            return alivePlayers;
        }*/
    public override bool ShowCustomRoleScreen() => true;

    public override void CanKill(out bool runOriginal, out bool result, PlayerControl target)
    {
        runOriginal = false;
        result = true;
    }

    public override bool CanReport(DeadBody body) => false;
    public override bool CanVent(Vent vent, GameData.PlayerInfo playerInfo) => false;
    public override bool ShouldShowSabotageMap(MapBehaviour map) => false;
    public override bool CanUseConsole(Console console) => false;
    public override bool CanUseMapConsole(MapConsole console) => false;
    public override bool CanUseSystemConsole(SystemConsole console) => false;
    public override void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = false;
        var alivePlayers = GameData.Instance.AllPlayers.ToArray().Where(player => !player.Disconnected && !player.IsDead);
        if (alivePlayers.Count() == 1)
        {
            instance.Manager.RpcEndGame(GameOverReason.ImpostorByKill, false);
        }
    }

    public override void AssignRoles(out bool runOriginal, LogicRoleSelectionNormal instance)
    {
        runOriginal = false;
        foreach (var player in GameData.Instance.AllPlayers)
        {
            player.Object.RpcSetRole(RoleTypes.Impostor);
        }
    }
}