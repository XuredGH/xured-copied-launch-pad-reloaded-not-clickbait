namespace LaunchpadReloaded.Networking;
public enum LaunchpadRPC : uint
{
    HackPlayer = 0,
    UnhackPlayer = 1,
    CreateNodes = 2,
    ToggleNode = 3,
    StartDrag = 3,
    StopDrag = 4,
    RemoveBody = 5,
    HideBodyInVent = 6,
    ExposeBody = 7,
    SyncGameOptions = 8
}