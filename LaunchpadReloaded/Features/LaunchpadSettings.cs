using LaunchpadReloaded.API.Settings;

namespace LaunchpadReloaded.Features;

public class LaunchpadSettings
{
    public static LaunchpadSettings Instance { get; private set; }

    public readonly CustomSetting LockedCamera;

    private LaunchpadSettings()
    {
        LockedCamera = new CustomSetting("Locked Camera");
    }

    public static void Initialize()
    {
        Instance = new LaunchpadSettings();
    }
}