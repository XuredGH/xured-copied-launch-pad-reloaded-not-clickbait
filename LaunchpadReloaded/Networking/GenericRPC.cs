using LaunchpadReloaded.Utilities;
using PowerTools;
using Reactor.Networking.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Networking;
public static class GenericRPC
{
    [MethodRpc((uint)LaunchpadRPC.SetBodyType)]
    public static void RpcSetBodyType(PlayerControl player, int bodyType)
    {
        if (bodyType == 6)
        {
            player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
            if (!LaunchpadGameOptions.Instance.ShowKnife.Value)
            {
                return;
            }

            var seekerHand = player.transform.FindChild("BodyForms/Seeker/SeekerHand").gameObject;
            var hand = Object.Instantiate(seekerHand).gameObject;
            hand.transform.SetParent(seekerHand.transform.parent);
            hand.transform.localScale = new Vector3(2, 2, 2);
            hand.name = "KnifeHand";
            hand.layer = LayerMask.NameToLayer("Players");

            var nodeSync = hand.GetComponent<SpriteAnimNodeSync>();
            nodeSync.flipOffset = new Vector3(-1.5f, 0.5f, 0.0113f);
            nodeSync.normalOffset = new Vector3(1.5f, 0.5f, 0.0113f);

            var rend = hand.GetComponent<SpriteRenderer>();
            rend.sprite = LaunchpadAssets.KnifeHandSprite.LoadAsset();

            hand.SetActive(true);
            return;
        }

        player.MyPhysics.SetBodyType((PlayerBodyTypes)bodyType);
    }
}