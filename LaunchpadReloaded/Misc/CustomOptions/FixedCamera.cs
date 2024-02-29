using LaunchpadReloaded.API.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Misc.CustomOptions;

public class FixedCamera : CustomOption
{
    public override string Text => "Fixed Camera";
    public override string Id => "FixCam";

    public override void OnClick()
    {
        Debug.Log(this.Enabled);
    }
}