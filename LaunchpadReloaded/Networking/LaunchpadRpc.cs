namespace LaunchpadReloaded.Networking;
public enum LaunchpadRpc : uint
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
    SyncGameOptions,
    SyncRoleOption,
    CustomCheckMurder,
    CustomMurder,
    CustomCheckColor,
    CustomSetColor,
    RemoveVote,
    PopulateResults
}