using LaunchpadReloaded.API.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchpadReloaded.Misc.CustomOptions;

public class Test : CustomOption
{
    public override string Text => "Test";
    public override string Id => "Test";

    public override void OnClick()
    {
        base.OnClick();
    }
}