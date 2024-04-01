namespace LaunchpadReloaded.Networking;
public enum LaunchpadRPC : uint
{
    HackPlayer,
    UnHackPlayer,
    CreateNodes,
    StartDrag,
    StopDrag,
    RemoveBody,
    HideBodyInVent,
    ExposeBody,
    SetGameMode,
    SetBodyType,
    CreateScanner,
    Revive,
    SyncGradient,
    SyncGameOptions,
    SyncRoleOption
}