using AmongUs.Data.Player;
using Il2CppInterop.Runtime;
using Il2CppSystem.Text.Json;
using LaunchpadReloaded.API.Hud;
using LaunchpadReloaded.API.Utilities;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using Reactor.Networking.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static GameData;
using static UnityEngine.GraphicsBuffer;

namespace LaunchpadReloaded.Buttons;
public class HackButton : CustomActionButton
{
    public override string Name => "HACK";
    public override float Cooldown => 60;
    public override float EffectDuration => 0;
    public override int MaxUses => 2;
    public override Sprite Sprite => SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Hack.png");
    public override bool Enabled(RoleBehaviour role) =>  role is HackerRole;
    public override bool CanUse() => !HackingManager.AnyActiveNodes();
    protected override void OnClick()
    {
        foreach (PlayerControl player in PlayerControl.AllPlayerControls)
        {
            HackingManager.RpcHackPlayer(player);
        }

        HackingManager.RpcToggleNode(ShipStatus.Instance, HackingManager.Nodes.Random().Id, true);
    }
}