using Reactor.Networking.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Networking;
public static class GenericRPC
{
    [MethodRpc((uint)LaunchpadRPC.SetPlayerPosition)]
    public static void RpcSetPlayerPosition(PlayerControl player, float x, float y, float z)
    {
        player.transform.position = new Vector3(x, y, z);
    }

    [MethodRpc((uint)LaunchpadRPC.SetBodyType)]
    public static void RpcSetBodyType(PlayerControl player, int bodyType)
    {
        player.MyPhysics.SetBodyType((PlayerBodyTypes)bodyType);
    }
}