using InnerNet;
using LaunchpadReloaded.Buttons;
using LaunchpadReloaded.Features;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp(typeof(IUsable))]
public class HackNodeComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public bool IsActive = false;
    public int Id;
    public SpriteRenderer Image;
    public ImageNames UseIcon => ImageNames.UseButton;
    public float UsableDistance => 3f;
    public float PercentCool => 0;

    public void SetOutline(bool on, bool mainTarget)
    {
        Image.material.SetFloat("_Outline", (float)(on ? 1 : 0));
        Image.material.SetColor("_OutlineColor", Color.green);
        Image.material.SetColor("_AddColor", mainTarget ? Color.green : Color.clear);
    }

    public void Use()
    {
        HackingManager.RpcUnhackPlayer(PlayerControl.LocalPlayer);
        if(HackingManager.HackedPlayers.Count <= 0)
        {
            HackingManager.RpcToggleNode(ShipStatus.Instance, this.Id, false);
        }
    }

    public float CanUse(GameData.PlayerInfo pc, out bool canUse, out bool couldUse)
    {
        float num = float.MaxValue;
        PlayerControl @object = pc.Object;
        couldUse = (!pc.IsDead && @object.CanMove && IsActive && HackingManager.HackedPlayers.Contains(pc.PlayerId));
        canUse = couldUse;
        if (canUse)
        {
            Vector2 truePosition = @object.GetTruePosition();
            Vector3 position = base.transform.position;
            num = Vector2.Distance(truePosition, position);
            canUse &= (num <= 0.5f && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false));
        }
        return num;
    }
}
