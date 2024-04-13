using System;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LaunchpadReloaded.Features;

public static class LaunchpadAssets
{
    public static readonly AssetBundle Bundle = AssetBundleManager.Load("assets");
    public static readonly LoadableAsset<Sprite> NoImage = new("", false);

    // Materials
    public static readonly LoadableAsset<Material> GradientMaterial = new("GradientPlayerMaterial");
    public static readonly LoadableAsset<Material> MaskedGradientMaterial = new("MaskedGradientMaterial");

    // Sprites
    public static readonly LoadableAsset<Sprite> BlankButton = new("BlankButton");
    public static readonly LoadableAsset<Sprite> CallButton = new("CallMeeting.png");
    public static readonly LoadableAsset<Sprite> DragButton = new("Drag.png");
    public static readonly LoadableAsset<Sprite> DropButton = new("Drop.png");
    public static readonly LoadableAsset<Sprite> ZoomButton = new("Zoom.png");
    public static readonly LoadableAsset<Sprite> ReviveButton = new("Revive.png");
    public static readonly LoadableAsset<Sprite> HideButton = new("Clean.png");
    public static readonly LoadableAsset<Sprite> HackButton = new("Hack.png");
    public static readonly LoadableAsset<Sprite> MapButton = new("Map.png");
    public static readonly LoadableAsset<Sprite> ScannerButton = new("Place_Scanner.png");
    public static readonly LoadableAsset<Sprite> TrackButton = new("Track.png");
    public static readonly LoadableAsset<Sprite> ShootButton = new("Shoot.png");
    public static readonly LoadableAsset<Sprite> JesterIcon = new("Jester.png");
    public static readonly LoadableAsset<Sprite> InstinctButton = new("Instinct.png", false);
    public static readonly LoadableAsset<Sprite> InvestigateButton = new("Investigate.png", false);

    // Object Sprites
    public static readonly LoadableAsset<Sprite> ScannerSprite = new("Scanner.png");
    public static readonly LoadableAsset<Sprite> NodeSprite = new("Node.png");
    public static readonly LoadableAsset<Sprite> KnifeHandSprite = new("KnifeHand.png");
    public static readonly LoadableAsset<Sprite> Footstep = new("Footstep.png", false);
    // Sounds
    public static readonly LoadableAsset<AudioClip> BeepSound = new("Beep.wav");
    public static readonly LoadableAsset<AudioClip> PingSound = new("Ping.mp3");
    public static readonly LoadableAsset<AudioClip> HackingSound = new("HackAmbience.mp3");

    // Other
    public static readonly LoadableAsset<GameObject> DetectiveGame = new("JournalMinigame");
}

public class LoadableAsset<T>(string name, bool useBundle = true) where T : Object
{
    private const string ResourcesFolder = "LaunchpadReloaded.Resources.";
    public string Name { get; } = name;
    public bool UseBundle { get; } = useBundle;

    private T _loadedAsset;

    public T LoadAsset()
    {
        if (_loadedAsset != null)
        {
            return _loadedAsset;
        }

        if (UseBundle)
        {
            return _loadedAsset = LaunchpadAssets.Bundle.LoadAsset<T>(Name);
        }

        if (typeof(T) == typeof(Sprite))
        {
            return _loadedAsset = SpriteTools.LoadSpriteFromPath(ResourcesFolder + Name) as T;
        }

        throw new Exception($"INVALID ASSET: {Name}");
    }
}