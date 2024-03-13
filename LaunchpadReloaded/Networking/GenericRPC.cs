using LaunchpadReloaded.API.Utilities;
using PowerTools;
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
    [MethodRpc((uint)LaunchpadRPC.SetBodyType)]
    public static void RpcSetBodyType(PlayerControl player, int bodyType)
    {
        if(bodyType == 6)
        {
            player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
            GameObject seekerHand = player.transform.FindChild("BodyForms/Seeker/SeekerHand").gameObject;
            GameObject hand = GameObject.Instantiate(seekerHand).gameObject;
            hand.transform.SetParent(seekerHand.transform.parent);
            hand.transform.localScale = new Vector3(2, 2, 2);
            hand.name = "KnifeHand";

            SpriteAnimNodeSync nodeSync = hand.GetComponent<SpriteAnimNodeSync>();
            nodeSync.flipOffset = new Vector3(-1.5f, 0.5f, -0.0228f);
            nodeSync.normalOffset = new Vector3(1.5f, 0.5f, -0.0228f);

            SpriteRenderer rend = hand.GetComponent<SpriteRenderer>();
            rend.sprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.KnifeHand.png");

            hand.SetActive(true);
            return;
        }

        player.MyPhysics.SetBodyType((PlayerBodyTypes)bodyType);
    }
}