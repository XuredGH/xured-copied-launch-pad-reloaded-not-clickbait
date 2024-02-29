using AmongUs.Data.Player;
using Il2CppInterop.Runtime;
using Il2CppSystem.Text.Json;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Tasks;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GameData;

namespace LaunchpadReloaded.Buttons;
public class HackButton : CustomActionButton
{
    public override string Name => "HACK";
    public override float Cooldown => 5;
    public override float EffectDuration => 5;
    public override int MaxUses => 3;
    public override string SpritePath => "Zoom.png";

    public static List<PlayerInfo> HackedPlayers = new List<PlayerInfo>();

    [MethodRpc((uint)LaunchpadRPC.HackPlayer)]
    public static void RpcHackPlayer(PlayerControl player)
    {
        HackedPlayers.Add(player.Data);
        player.RawSetName("<b><i>???</b></i>");
        player.RawSetColor(15);

        if (GameData.Instance.GetHost() == player.Data)
        {
            ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, 128);
        }

        var task = PlayerTask.GetOrCreateTask<HackSabotage>(player);
        task.Id = 255U;
        task.Owner = player;
        task.Initialize();
    }

    [MethodRpc((uint)LaunchpadRPC.UnhackPlayer)]
    public static void RpcUnhackPlayer(PlayerControl player)
    {
        HackedPlayers.Remove(player.Data);
        player.SetName(player.Data.PlayerName);
        player.SetColor((byte)player.Data.DefaultOutfit.ColorId);
    }

    public override bool Enabled(RoleBehaviour role)
    {
        return role is HackerRole;
    }

    protected override void OnClick()
    {
        foreach (PlayerControl player in PlayerControl.AllPlayerControls)
        {
            RpcHackPlayer(player);
        }
    }
}