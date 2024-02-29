using Il2CppSystem;
using LaunchpadReloaded.Buttons;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Tasks;

[RegisterInIl2Cpp]
public class HackSabotage(System.IntPtr ptr) : HudOverrideTask(ptr)
{
    private ArrowBehaviour arrow;

    public void CreateArrow(PlayerTask originalTask, Transform parent)
    {
        // Arrow Buttons
        GameObject arrowClone = originalTask.gameObject.transform.FindChild("Arrow").gameObject; 
        SpriteRenderer arrowCloneSprite = arrowClone.GetComponent<SpriteRenderer>();
        GameObject arrowObj = new GameObject(name: "Arrow");

        // Sprite
        SpriteRenderer arrowSprite = arrowObj.AddComponent<SpriteRenderer>();
        arrowSprite.material = arrowCloneSprite.material;
        arrowSprite.sprite = arrowCloneSprite.sprite;
        arrowObj.layer = LayerMask.NameToLayer("UI");

        // Arrow Behaviour
        ArrowBehaviour arrowBehaviour = arrowObj.AddComponent<ArrowBehaviour>(); 
        arrowBehaviour.image = arrowSprite;

        // Transform
        arrowObj.transform.SetParent(parent);
        arrowObj.transform.localScale = new Vector3(x: 0.4f, y: 0.4f, z: 1.0f); 

        this.arrow = arrowBehaviour;
    }

    public override void Initialize()
    {
        Debug.Log("Initialize is being called on local client.");
        PlayerTask originalTask = ShipStatus.Instance.GetSabotageTask(SystemTypes.Comms);
        ShipStatus instance = ShipStatus.Instance;
        this.system = instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
        this.MinigamePrefab = originalTask.GetMinigamePrefab();

        if(base.Owner && base.Owner.AmOwner)
        {
            CreateArrow(originalTask, this.transform);
            arrow.target = FindConsoles()[0].transform.position;
            arrow.gameObject.SetActive(true);
        }
    }

    public override void AppendTaskText(Il2CppSystem.Text.StringBuilder sb)
    {
        this.even = !this.even;
        Color color = this.even ? new Color32(14, 107, 14, 255) : new Color32(20, 148, 20, 255);
        sb.Append(color.ToTextColor());
        sb.Append("Comms Hacked\n");
        sb.Append("You will not be able to complete tasks until comms is fixed.\n");
        sb.Append("</color>");

        if(arrow != null)
            arrow.image.color = color;
    }

    public override void Complete()
    {
        PlayerControl.LocalPlayer.RemoveTask(this);
        HackButton.RpcUnhackPlayer(PlayerControl.LocalPlayer);
    }
}
