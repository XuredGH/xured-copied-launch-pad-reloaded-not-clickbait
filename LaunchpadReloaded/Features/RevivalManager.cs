using AmongUs.GameOptions;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Features;

[RegisterInIl2Cpp]
public class RevivalManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static RevivalManager Instance;
    public List<byte> RevivedPlayers = [];

    private void Awake()
    {
        Instance = this;
    }

    [MethodRpc((uint)LaunchpadRPC.Revive)]
    public static void RpcRevive(ShipStatus shipStatus, byte bodyId)
    {
        var body = DeadBodyManager.GetBodyById(bodyId);
        if (body)
        {
            Revive(body);
        }
        else
        {
            Debug.LogWarning($"Body for id {bodyId} not found");
        }
    }

    public static void Revive(DeadBody body)
    {
        var player = PlayerControl.AllPlayerControls.ToArray().ToList().Find(player => player.PlayerId == body.ParentId);
        player.transform.position = body.transform.position;
        player.Revive();

        player.RemainingEmergencies = GameManager.Instance.LogicOptions.GetNumEmergencyMeetings();
        RoleManager.Instance.SetRole(player, RoleTypes.Crewmate);
        player.Data.Role.SpawnTaskHeader(player);
        player.MyPhysics.SetBodyType(player.BodyType);

        if (player.AmOwner)
        {
            HudManager.Instance.MapButton.gameObject.SetActive(true);
            HudManager.Instance.ReportButton.gameObject.SetActive(true);
            HudManager.Instance.UseButton.gameObject.SetActive(true);
            player.myTasks.RemoveAt(0);
        }

        body.gameObject.Destroy();
        Instance.RevivedPlayers.Add(player.PlayerId);
    }
}