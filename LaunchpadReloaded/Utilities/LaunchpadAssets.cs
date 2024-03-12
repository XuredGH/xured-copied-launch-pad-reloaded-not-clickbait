using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;

public static class LaunchpadAssets
{
    public static AssetBundle Bundle = AssetBundleManager.Load("assets");

    // Button Sprites
    public static LoadableAsset<Sprite> CallButton = new("CallMeeting.png");
    public static LoadableAsset<Sprite> DragButton = new("Drag.png");
    public static LoadableAsset<Sprite> DropButton = new("Drop.png");
    public static LoadableAsset<Sprite> ZoomButton = new("Zoom.png");
    public static LoadableAsset<Sprite> ReviveButton = new("Revive.png");
    public static LoadableAsset<Sprite> HideButton = new("Clean.png");
    public static LoadableAsset<Sprite> HackButton = new("Hack.png", false);
    public static LoadableAsset<Sprite> MapButton = new("Map.png", false);
    public static LoadableAsset<Sprite> ScannerButton = new("Place_Scanner.png", false);
    public static LoadableAsset<Sprite> TrackButton = new("Track.png", false);

    // Object Sprites
    public static LoadableAsset<Sprite> ScannerSprite = new("Scanner.png", false);
    public static LoadableAsset<Sprite> NodeSprite = new("Node.png", false);

    // Sounds
    public static LoadableAsset<AudioClip> BeepSound = new("Beep.wav");
    public static LoadableAsset<AudioClip> PingSound = new("Ping.mp3");
}

public class LoadableAsset<T> where T : Object
{
    public string Name { get; }
    public bool UseBundle { get; }
    private string ResourcesFolder = "LaunchpadReloaded.Resources.";
    private T LoadedAsset = null;

    public LoadableAsset(string name, bool useBundle = true)
    {
        Name = name;
        UseBundle = useBundle;
    }
    public T LoadAsset()
    {
        if (LoadedAsset != null) return LoadedAsset;

        if (UseBundle) return LoadedAsset = LaunchpadAssets.Bundle.LoadAsset<T>(Name);
        else return LoadedAsset = SpriteTools.LoadSpriteFromPath(ResourcesFolder + Name) as T;
    }
}