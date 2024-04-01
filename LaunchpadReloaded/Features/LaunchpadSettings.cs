using LaunchpadReloaded.API.Settings;

namespace LaunchpadReloaded.Features;

public class LaunchpadSettings
{
    private static LaunchpadSettings _instance;

    public static LaunchpadSettings Instance
    {
        get { return _instance ??= new LaunchpadSettings(); }
    }

    public readonly CustomSetting LockedCamera;

    private LaunchpadSettings()
    {
        LockedCamera = new CustomSetting("Locked Camera");
    }

    public static void Initialize()
    {
        _instance = new LaunchpadSettings();
    }
}