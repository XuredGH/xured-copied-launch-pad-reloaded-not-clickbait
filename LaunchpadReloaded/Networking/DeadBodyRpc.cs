using System.Linq;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Networking;

public static class DeadBodyRpc
{
    [MethodRpc((uint)LaunchpadRpc.HideBodyInVent)]
    public static void RpcHideBodyInVent(this PlayerControl pc, byte bodyId, int ventId)
    {
        if (pc.Data.Role is not JanitorRole)
        {
            return;
        }
        
        var body = Helpers.GetBodyById(bodyId);
        var vent = ShipStatus.Instance.AllVents.First(v => v.Id == ventId);

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
    
    [MethodRpc((uint)LaunchpadRpc.ExposeBody)]
    public static void RpcExposeBody(this PlayerControl playerControl, int ventId)
    {
        if (!playerControl.Data.Role.CanVent)
        {
            return;
        }
        
        var vent = ShipStatus.Instance.AllVents.First(v => v.Id == ventId);

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