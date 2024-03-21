using LaunchpadReloaded.Features;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp(typeof(IUsable))]
public class HackNodeComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    public bool IsActive;
    public int Id;
    public SpriteRenderer Image;
    public ImageNames UseIcon => ImageNames.UseButton;
    public float UsableDistance => 0.8f;
    public float PercentCool => 0;

    public void SetOutline(bool on, bool mainTarget)
    {
        Image.material.SetFloat(ShaderID.Outline, on ? 1 : 0);
        Image.material.SetColor(ShaderID.OutlineColor, Color.green);
        Image.material.SetColor(ShaderID.AddColor, mainTarget ? Color.green : Color.clear);
    }

    public void Use()
    {
        SoundManager.Instance.PlaySound(LaunchpadAssets.BeepSound.LoadAsset(), false, 0.5f);
        HackingManager.RpcUnHackPlayer(PlayerControl.LocalPlayer);
    }

    public float CanUse(GameData.PlayerInfo pc, out bool canUse, out bool couldUse)
    {
        var num = float.MaxValue;
        var @object = pc.Object;
        couldUse = (!pc.IsDead && @object.CanMove && IsActive && pc.IsHacked());
        canUse = couldUse;
        if (canUse)
        {
            var truePosition = @object.GetTruePosition();
            var position = transform.position;
            num = Vector2.Distance(truePosition, position);
            canUse &= (num <= UsableDistance);
        }
        return num;
    }
}
