using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Features.Managers;

[RegisterInIl2Cpp]
public class RevivalManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static RevivalManager Instance;
    public List<byte> RevivedPlayers;

    private void Awake()
    {
        Instance = this;
        RevivedPlayers = new List<byte>();
    }

    private void OnDestroy()
    {
        RevivedPlayers.Clear();
    }

    [MethodRpc((uint)LaunchpadRPC.Revive)]
    public static void RpcRevive(PlayerControl playerControl, byte bodyId)
    {
        if (playerControl.Data.Role is not MedicRole)
        {
            return;
        }
        
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
        player.NetTransform.SnapTo(body.transform.position);
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