using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Attributes;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Features;

public static class DeadBodyManager
{
    public static DeadBody GetBodyById(byte id)
    {
        return Object.FindObjectsOfType<DeadBody>().FirstOrDefault(body => body.ParentId == id);
    }

    [MethodRpc((uint)LaunchpadRPC.HideBodyInVent)]
    public static void RpcHideBodyInVent(ShipStatus shipStatus, byte bodyId, int ventId)
    {
        var body = GetBodyById(bodyId);
        var vent = shipStatus.AllVents.First(v => v.Id == ventId);

        if (!body || !vent)
        {
            return;
        }

        var ventBody = vent.GetComponent<VentBodyComponent>();
        if (!ventBody)
        {
            ventBody = vent.gameObject.AddComponent<VentBodyComponent>();
        }

        body.HideBody();
        var pos = body.transform.position;
        var pos2 = vent.transform.position;
        body.transform.position = new Vector3(pos2.x, pos2.y, pos.z);
        ventBody.deadBody = body;
    }

    [MethodRpc((uint)LaunchpadRPC.RemoveBody)]
    public static void RpcRemoveBody(ShipStatus shipStatus, byte bodyId)
    {
        var body = GetBodyById(bodyId);

        if (!body)
        {
            return;
        }

        body.HideBody();
    }

    [MethodRpc((uint)LaunchpadRPC.ExposeBody)]
    public static void RpcExposeBody(ShipStatus shipStatus, int ventId)
    {
        var vent = shipStatus.AllVents.First(v => v.Id == ventId);

        if (!vent)
        {
            return;
        }

        var ventBody = vent.GetComponent<VentBodyComponent>();
        if (!ventBody)
        {
            return;
        }

        ventBody.ExposeBody();
    }

}