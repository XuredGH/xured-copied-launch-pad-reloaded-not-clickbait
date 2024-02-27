using LaunchpadReloaded.API.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Misc.CustomOptions;

public class SabotageShake : CustomOption
{
    public override string Text => "Sabotage Camera Shake";
    public override string Id => "SabShake";

    public override void OnClick()
    {
        base.OnClick();
        Debug.Log(this.Enabled);
    }
}