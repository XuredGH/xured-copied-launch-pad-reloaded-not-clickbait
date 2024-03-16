namespace LaunchpadReloaded.Networking;
public enum LaunchpadRPC : uint
{
    HackPlayer = 0,
    UnHackPlayer = 1,
    CreateNodes = 2,
    ToggleNode = 4,
    StartDrag = 5,
    StopDrag = 6,
    RemoveBody = 7,
    HideBodyInVent = 8,
    ExposeBody = 9,
    SyncGameOptions = 10,
    SetGameMode = 11,
    SetBodyType = 12,
    CreateScanner = 13,
    Revive = 14,
    SyncGradient = 15
}