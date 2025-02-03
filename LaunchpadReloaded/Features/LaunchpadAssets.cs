using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Features;

public static class LaunchpadAssets
{
    public static readonly AssetBundle Bundle = AssetBundleManager.Load("assets");

    // Materials
    public static readonly LoadableAsset<Material> GradientMaterial = new LoadableBundleAsset<Material>("GradientPlayerMaterial", Bundle);
    public static readonly LoadableAsset<Material> MaskedGradientMaterial = new LoadableBundleAsset<Material>("MaskedGradientMaterial", Bundle);

    // Sprites
    public static readonly LoadableAsset<Sprite> BlankButton = new LoadableBundleAsset<Sprite>("BlankButton", Bundle);
    public static readonly LoadableAsset<Sprite> CallButton = new LoadableBundleAsset<Sprite>("CallMeeting.png", Bundle);
    public static readonly LoadableAsset<Sprite> DissectButton = new LoadableResourceAsset("LaunchpadReloaded.Resources.Dissect.png");
    public static readonly LoadableAsset<Sprite> DragButton = new LoadableBundleAsset<Sprite>("Drag.png", Bundle);
    public static readonly LoadableAsset<Sprite> DropButton = new LoadableBundleAsset<Sprite>("Drop.png", Bundle);
    public static readonly LoadableAsset<Sprite> HackButton = new LoadableBundleAsset<Sprite>("Hack.png", Bundle);
    public static readonly LoadableAsset<Sprite> HideButton = new LoadableBundleAsset<Sprite>("Clean.png", Bundle);
    public static readonly LoadableAsset<Sprite> InjectButton = new LoadableResourceAsset("LaunchpadReloaded.Resources.Inject.png");
    public static readonly LoadableAsset<Sprite> InstinctButton = new LoadableResourceAsset("LaunchpadReloaded.Resources.Instinct.png");
    public static readonly LoadableAsset<Sprite> InvestigateButton = new LoadableResourceAsset("LaunchpadReloaded.Resources.Investigate.png");
    public static readonly LoadableAsset<Sprite> JesterIcon = new LoadableBundleAsset<Sprite>("Jester.png", Bundle);
    public static readonly LoadableAsset<Sprite> MapButton = new LoadableBundleAsset<Sprite>("Map.png", Bundle);
    public static readonly LoadableAsset<Sprite> ReviveButton = new LoadableBundleAsset<Sprite>("Revive.png", Bundle);
    public static readonly LoadableAsset<Sprite> ScannerButton = new LoadableBundleAsset<Sprite>("Place_Scanner.png", Bundle);
    public static readonly LoadableAsset<Sprite> ShootButton = new LoadableBundleAsset<Sprite>("Shoot.png", Bundle);
    public static readonly LoadableAsset<Sprite> TrackButton = new LoadableBundleAsset<Sprite>("Track.png", Bundle);
    public static readonly LoadableAsset<Sprite> ZoomButton = new LoadableBundleAsset<Sprite>("Zoom.png", Bundle);

    public static readonly LoadableAsset<Sprite> NotepadSprite = new LoadableResourceAsset("LaunchpadReloaded.Resources.NotepadButton.png");
    public static readonly LoadableAsset<Sprite> NotepadActiveSprite = new LoadableResourceAsset("LaunchpadReloaded.Resources.NotepadButtonActive.png");

    // Banner Sprites
    public static readonly LoadableAsset<Sprite> CaptainBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Captain.png");
    public static readonly LoadableAsset<Sprite> DetectiveBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Detective.png");
    public static readonly LoadableAsset<Sprite> HackerBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Hacker.png");
    public static readonly LoadableAsset<Sprite> JanitorBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Janitor.png");
    public static readonly LoadableAsset<Sprite> JesterBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Jester.png");
    public static readonly LoadableAsset<Sprite> MayorBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Mayor.png");
    public static readonly LoadableAsset<Sprite> MedicBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Medic.png");
    public static readonly LoadableAsset<Sprite> SheriffBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Sheriff.png");
    public static readonly LoadableAsset<Sprite> SurgeonBanner = new LoadableResourceAsset("LaunchpadReloaded.Resources.Banners.Surgeon.png");

    // Object Sprites
    public static readonly LoadableAsset<Sprite> Arrow = new LoadableResourceAsset("LaunchpadReloaded.Resources.Arrow.png");
    public static readonly LoadableAsset<Sprite> Bone = new LoadableResourceAsset("LaunchpadReloaded.Resources.Bone.png");
    public static readonly LoadableAsset<Sprite> Footstep = new LoadableResourceAsset("LaunchpadReloaded.Resources.Footstep.png");
    public static readonly LoadableAsset<Sprite> KnifeHandSprite = new LoadableBundleAsset<Sprite>("KnifeHand.png", Bundle);
    public static readonly LoadableAsset<Sprite> NodeSprite = new LoadableBundleAsset<Sprite>("Node.png", Bundle);
    public static readonly LoadableAsset<Sprite> ScannerSprite = new LoadableBundleAsset<Sprite>("Scanner.png", Bundle);

    // Sounds
    public static readonly LoadableAsset<AudioClip> BeepSound = new LoadableBundleAsset<AudioClip>("Beep.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> InjectSound = new LoadableBundleAsset<AudioClip>("Inject.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> DissectSound = new LoadableBundleAsset<AudioClip>("Dissect.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> HackingSound = new LoadableBundleAsset<AudioClip>("HackAmbience.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> PingSound = new LoadableBundleAsset<AudioClip>("Ping.mp3", Bundle);

    // Other
    public static readonly LoadableAsset<GameObject> DetectiveGame = new LoadableBundleAsset<GameObject>("JournalMinigame", Bundle);
    public static readonly LoadableAsset<GameObject> Notepad = new LoadableBundleAsset<GameObject>("Notepad", Bundle);
    public static readonly LoadableAsset<GameObject> NodeGame = new LoadableBundleAsset<GameObject>("NodeMinigame", Bundle);
}