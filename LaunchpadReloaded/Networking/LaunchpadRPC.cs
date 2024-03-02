﻿namespace LaunchpadReloaded.Networking;

public enum LaunchpadRPC : uint
{
    HackPlayer = 0,
    UnhackPlayer = 1,
    StartDrag = 3,
    StopDrag = 4,
    HideBody = 5,
    HideBodyInVent = 6,
    ExposeBody = 7,
}