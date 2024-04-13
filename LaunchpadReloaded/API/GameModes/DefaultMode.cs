using LaunchpadReloaded.Features;

namespace LaunchpadReloaded.API.GameModes;

[RegisterGameMode]
public class DefaultMode : CustomGameMode
{
    public override string Name => "Default";
    public override string Description => "Default Among Us GameMode";
    public override int Id => (int)LaunchpadGamemodes.Default;
}