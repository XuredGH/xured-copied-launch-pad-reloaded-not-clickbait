namespace LaunchpadReloaded.Networking.Murder;

public struct CustomMurderData(PlayerControl playerControl, MurderResultFlags murderResultFlags)
{
    public readonly PlayerControl TargetPlayer = playerControl;
    public readonly MurderResultFlags MurderResultFlags = murderResultFlags;
}