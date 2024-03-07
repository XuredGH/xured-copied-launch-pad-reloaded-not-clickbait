using LaunchpadReloaded.API.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LaunchpadReloaded.Utilities;
public static class LaunchpadAssets
{
    public static AssetBundle Bundle;

    #region Button Sprites
    public static Sprite CallButton;
    public static Sprite DragButton;
    public static Sprite DropButton;
    public static Sprite HackButton;
    public static Sprite HideButton;
    public static Sprite MapButton;
    public static Sprite ReviveButton;
    public static Sprite ScannerButton;
    public static Sprite TrackButton;
    public static Sprite ZoomButton;
    #endregion
    #region Object Sprites
    public static Sprite ScannerSprite;
    public static Sprite NodeSprite;
    #endregion
    #region Sounds
    public static AudioClip BeepSound;
    public static AudioClip PingSound;
    #endregion

    public static void Load()
    {
        Bundle = AssetBundleManager.Load("assets");

        // Asset Bundle
        CallButton = Bundle.LoadAsset<Sprite>("CallMeeting.png");
        DragButton = Bundle.LoadAsset<Sprite>("Drag.png");
        DropButton = Bundle.LoadAsset<Sprite>("Drop.png");
        ZoomButton = Bundle.LoadAsset<Sprite>("Zoom.png");
        ReviveButton = Bundle.LoadAsset<Sprite>("Revive.png");
        HideButton = Bundle.LoadAsset<Sprite>("Clean.png");

        // Resources
        HackButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Hack.png");
        MapButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Map.png");
        ScannerButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Place_Scanner.png");
        TrackButton = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Track.png");
        ScannerSprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Scanner.png");
        NodeSprite = SpriteTools.LoadSpriteFromPath("LaunchpadReloaded.Resources.Node.png");

        // Sounds
        BeepSound = Bundle.LoadAsset<AudioClip>("Beep.wav");
        PingSound = Bundle.LoadAsset<AudioClip>("Ping.mp3");
    }
}