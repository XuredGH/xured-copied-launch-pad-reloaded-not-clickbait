using LaunchpadReloaded.API.Settings;

namespace LaunchpadReloaded.CustomSettings;

public class LaunchpadSettings
{
    public static LaunchpadSettings Instance { get; private set; }

    public CustomSetting LockedCamera;

    public LaunchpadSettings()
    {
        LockedCamera = new CustomSetting("Locked Camera");

        Instance = this;
    }
}