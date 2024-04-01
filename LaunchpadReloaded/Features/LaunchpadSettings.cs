﻿using Il2CppSystem;
using LaunchpadReloaded.API.Settings;
using System.Collections.Generic;
using System.Linq;

namespace LaunchpadReloaded.Features;

public class LaunchpadSettings
{
    private static LaunchpadSettings _instance;

    public static LaunchpadSettings Instance
    {
        get { return _instance ??= new LaunchpadSettings(); }
    }

    public readonly CustomSetting LockedCamera;
    public readonly CustomSetting UniqueDummies;

    private LaunchpadSettings()
    {
        LockedCamera = new CustomSetting("Locked Camera", false);
        UniqueDummies = new CustomSetting("Unique Dummies", true)
        {
            ChangedEvent = val =>
            {
                if (!TutorialManager.InstanceExists || !AccountManager.InstanceExists) return;
                List<DummyBehaviour> dummies = UnityEngine.Object.FindObjectsOfType<DummyBehaviour>().ToArray().Reverse().ToList();

                for (int i = 0; i < dummies.Count; i++)
                {
                    DummyBehaviour dummy = dummies[i];
                    if (!dummy.myPlayer) continue;

                    dummy.myPlayer.SetName(val ? AccountManager.Instance.GetRandomName() :
                        DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.Dummy, Array.Empty<Object>()) + " " + (i).ToString(), true);
                }
            }
        };
    }

    public static void Initialize()
    {
        _instance = new LaunchpadSettings();
    }
}