using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;
public static class LaunchpadAssets
{
    public static AssetBundle Bundle = AssetBundleManager.Load("assets");

    // Button Sprites
    public static Sprite CallButton = Bundle.LoadAsset<Sprite>("CallMeeting.png");
    public static Sprite DragButton = Bundle.LoadAsset<Sprite>("Drag.png");
    public static Sprite DropButton = Bundle.LoadAsset<Sprite>("Drop.png");
    public static Sprite ZoomButton = Bundle.LoadAsset<Sprite>("Zoom.png");
    public static Sprite ReviveButton = Bundle.LoadAsset<Sprite>("Revive.png");
    public static Sprite HideButton = Bundle.LoadAsset<Sprite>("Clean.png");
    public static Sprite HackButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Hack.png");
    public static Sprite MapButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Map.png");
    public static Sprite ScannerButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Place_Scanner.png");
    public static Sprite TrackButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Track.png");

    // Object Sprites
    public static Sprite ScannerSprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Scanner.png");
    public static Sprite NodeSprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Node.png");

    // Sounds
    public static AudioClip BeepSound = Bundle.LoadAsset<AudioClip>("Beep.wav");
    public static AudioClip PingSound = Bundle.LoadAsset<AudioClip>("Ping.mp3");
}