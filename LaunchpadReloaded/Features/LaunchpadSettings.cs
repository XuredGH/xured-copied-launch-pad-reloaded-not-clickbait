using Il2CppSystem;
using LaunchpadReloaded.API.Settings;
using System.Linq;

namespace LaunchpadReloaded.Features;

public class LaunchpadSettings
{
    public static LaunchpadSettings? Instance { get; private set; }

    public readonly CustomSetting LockedCamera;
    public readonly CustomSetting UniqueDummies;
#if !ANDROID
    public readonly CustomSetting ButtonLocation;
#endif

    private LaunchpadSettings()
    {
#if !ANDROID
        // TODO: update button location mid-game
        ButtonLocation = new CustomSetting("Buttons On Left", true);
#endif
        LockedCamera = new CustomSetting("Locked Camera", false);
        UniqueDummies = new CustomSetting("Unique Dummies", true)
        {
            ChangedEvent = val =>
            {
                if (!TutorialManager.InstanceExists || !AccountManager.InstanceExists)
                {
                    return;
                }

                var dummies = UnityEngine.Object.FindObjectsOfType<DummyBehaviour>().ToArray().Reverse().ToList();

                for (var i = 0; i < dummies.Count; i++)
                {
                    var dummy = dummies[i];
                    if (!dummy.myPlayer)
                    {
                        continue;
                    }

                    dummy.myPlayer.SetName(val ? AccountManager.Instance.GetRandomName() :
                        DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Dummy, Array.Empty<Object>()) + " " + i);
                }
            }
        };
    }

    public static void Initialize()
    {
        Instance = new LaunchpadSettings();
    }
}