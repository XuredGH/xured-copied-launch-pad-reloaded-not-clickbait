using Il2CppSystem;
using LaunchpadReloaded.API.Settings;
using MiraAPI.PluginLoading;
using System.Linq;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = Il2CppSystem.Object;

namespace LaunchpadReloaded.Features;

public class LaunchpadSettings
{
    public static LaunchpadSettings? Instance { get; private set; }

    public readonly CustomSetting Bloom;
    public readonly CustomSetting LockedCamera;
    public readonly CustomSetting UniqueDummies;
#if !ANDROID
    public readonly CustomSetting ButtonLocation;
#endif

    private LaunchpadSettings()
    {
#if !ANDROID
        // TODO: update button location mid-game
        ButtonLocation = new CustomSetting("Buttons On Left", true)
        {
            ChangedEvent = val =>
            {
                var plugin = MiraPluginManager.GetPluginByGuid(LaunchpadReloadedPlugin.Id);
                foreach (var button in plugin.GetButtons())
                {
                    button.SetButtonLocation(val ? MiraAPI.Hud.ButtonLocation.BottomLeft : MiraAPI.Hud.ButtonLocation.BottomRight);
                }
            }
        };
#endif
        Bloom = new CustomSetting("Bloom", true)
        {
            ChangedEvent = enabled =>
            {
                if (!GameData.Instance || Camera.main == null)
                {
                    return;
                }
                var bloom = Camera.main.GetComponent<Bloom>();
                if (bloom == null)
                {
                    bloom = Camera.main.gameObject.AddComponent<Bloom>();
                }
                bloom.enabled = enabled;
                bloom.SetBloomByMap();
            }
        };
        
        LockedCamera = new CustomSetting("Locked Camera", false);
        UniqueDummies = new CustomSetting("Unique Freeplay Dummies", true)
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