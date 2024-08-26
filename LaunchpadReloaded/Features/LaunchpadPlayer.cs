using LaunchpadReloaded.Networking;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Options;
using LaunchpadReloaded.Options.Roles;
using MiraAPI.GameOptions;
using UnityEngine;

namespace LaunchpadReloaded.Features;

[RegisterInIl2Cpp]
public class LaunchpadPlayer(IntPtr ptr) : MonoBehaviour(ptr)
{
    public Transform knife;

    public PlayerControl playerObject;
    
    public CustomVoteData VoteData;
    
    public DeathData DeadData;
    
    public bool showFootsteps;

    public bool wasRevived;

    public byte dragId = 255;
    
    public bool Dragging => dragId != 255;

    private CustomGameData.CustomPlayerInfo _cachedData;
    
    public CustomGameData.CustomPlayerInfo Data
    {
        get
        {
            return _cachedData ??= CustomGameData.Instance.GetPlayerById(playerObject.PlayerId);
        }
    }

    public static LaunchpadPlayer LocalPlayer { get; private set; }
    
    public static LaunchpadPlayer GetById(byte id) => GameData.Instance.GetPlayerById(id).Object.GetLpPlayer();
    
    public static IEnumerable<LaunchpadPlayer> GetAllPlayers() => PlayerControl.AllPlayerControls.ToArray().Select(player => player.GetLpPlayer());
    
    public static IEnumerable<LaunchpadPlayer> GetAllAlivePlayers() => GetAllPlayers().Where(plr => !plr.playerObject.Data.IsDead && !plr.playerObject.Data.Disconnected);
    
    private Vector3 _lastPos;

    private void Awake()
    {
        playerObject = gameObject.GetComponent<PlayerControl>();
        VoteData = new CustomVoteData();
        DeadData = new DeathData();

        if (playerObject.AmOwner)
        {
            LocalPlayer = this;
        }
        
        CustomGameData.Instance.AddPlayer(this);

        _lastPos = transform.position;
    }

    private void Update()
    {
        if (LocalPlayer.showFootsteps && PlayerControl.LocalPlayer.Data is not null && PlayerControl.LocalPlayer.Data.Role is DetectiveRole)
        {
            if (Vector3.Distance(_lastPos, transform.position) > 1)
            {
                var angle = Mathf.Atan2(playerObject.MyPhysics.Velocity.y, playerObject.MyPhysics.Velocity.x) * Mathf.Rad2Deg;

                var footstep = new GameObject("Footstep")
                {
                    transform =
                    {
                        parent = ShipStatus.Instance.transform,
                        position = new Vector3(transform.position.x, transform.position.y, 2.5708f),
                        rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward)
                    }
                };

                var sprite = footstep.AddComponent<SpriteRenderer>();
                sprite.sprite = LaunchpadAssets.Footstep.LoadAsset();
                sprite.material = LaunchpadAssets.GradientMaterial.LoadAsset();
                footstep.layer = LayerMask.NameToLayer("Players");

                sprite.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                playerObject.SetPlayerMaterialColors(sprite);

                _lastPos = transform.position;
                Destroy(footstep, OptionGroupSingleton<DetectiveOptions>.Instance.FootstepsDuration);
            }
        }
    }

    public void ResetPlayer()
    {
        wasRevived = false;
        dragId = 255;
    }

    public void OnDeath(PlayerControl killer)
    {
        if (playerObject.Data.IsHacked() && playerObject.AmOwner)
        {
            playerObject.RpcUnHackPlayer();
        }

        DeadData.DeathTime = DateTime.Now;
        DeadData.Killer = killer;
        DeadData.Suspects = PlayerControl.AllPlayerControls.ToArray().Where((pc) => pc != DeadData.Killer && !pc.Data.IsDead).Take(4).ToList();
    }

    private void FixedUpdate()
    {
        if (MeetingHud.Instance)
        {
            return;
        }

        if (playerObject.Data.IsHacked())
        {
            var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6), "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
            playerObject.cosmetics.SetName(randomString);
            playerObject.cosmetics.SetNameMask(true);
            playerObject.cosmetics.gameObject.SetActive(false);
        }
        else if (wasRevived)
        {
            playerObject.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.MedicColor));
        }

        knife = playerObject.gameObject.transform.FindChild("BodyForms/Seeker/KnifeHand");
        if (knife)
        {
            knife.gameObject.SetActive(!playerObject.Data.IsDead && playerObject.CanMove);
        }
    }
    
    public struct CustomVoteData()
    {
        public readonly List<byte> VotedPlayers = [];
        public int VotesRemaining = (int)OptionGroupSingleton<VotingOptions>.Instance.MaxVotes.Value;
    }

    public struct DeathData
    {
        public DateTime DeathTime;
        public PlayerControl Killer;
        public List<PlayerControl> Suspects;
    }
}