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
    public override void AppendTaskText(Il2CppSystem.Text.StringBuilder sb)
    {
        sb.Append(Color.green.ToTextColor());
        sb.Append("Comms Hacked");
        sb.Append("</color>");
    }

    

    public override void Initialize()
    {
        ShipStatus instance = ShipStatus.Instance;
        this.system = instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>();
    }

    public override void Complete()
    {
        this.isComplete = true;
        PlayerControl.LocalPlayer.RemoveTask(this);
        HackButton.RpcUnhackPlayer(PlayerControl.LocalPlayer);
    }
}
