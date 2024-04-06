using LaunchpadReloaded.Features;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using PowerTools;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Networking;
public static class GenericRpc
{
    [MethodRpc((uint)LaunchpadRpc.Revive)]
    public static void RpcRevive(this PlayerControl playerControl, byte bodyId)
    {
        if (playerControl.Data.Role is not MedicRole)
        {
            playerControl.KickForCheating();
        }

        var body = Helpers.GetBodyById(bodyId);
        if (body)
        {
            body.Revive();
        }
        else
        {
            Logger<LaunchpadReloadedPlugin>.Warning($"Body for id {bodyId} not found");
        }
    }


    [MethodRpc((uint)LaunchpadRpc.SetBodyType)]
    public static void RpcSetBodyType(this GameData gameData, PlayerControl player, int bodyType)
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

            var transform = player.transform;

            hand.transform.localPosition = transform.localPosition;
            hand.transform.position = transform.position;

            var nodeSync = hand.GetComponent<SpriteAnimNodeSync>();
            nodeSync.flipOffset = new Vector3(-1.5f, 0.5f, 0);
            nodeSync.normalOffset = new Vector3(1.5f, 0.5f, 0);

            var rend = hand.GetComponent<SpriteRenderer>();
            rend.sprite = LaunchpadAssets.KnifeHandSprite.LoadAsset();

            hand.SetActive(true);
            return;
        }

        player.MyPhysics.SetBodyType((PlayerBodyTypes)bodyType);
    }
}