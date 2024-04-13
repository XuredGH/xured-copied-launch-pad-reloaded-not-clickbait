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
    CreateScanner,
    Revive,
    SyncGameOptions,
    SyncRoleOption,
    CustomCheckMurder,
    CustomMurder,
    CustomCheckColor,
    CustomSetColor,
    SyncAllColors,
    RemoveVote,
    PopulateResults
}