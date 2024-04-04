using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Networking;
using UnityEngine;

namespace LaunchpadReloaded.Features;

[RegisterInIl2Cpp]
public class LaunchpadPlayer(IntPtr ptr) : MonoBehaviour(ptr)
{
    public List<byte> votedPlayers;
    public int votesRemaining;
    public bool didSkip;

    public Transform knife;

    public PlayerControl player;
    public static LaunchpadPlayer LocalPlayer;
    public static LaunchpadPlayer GetById(byte id) => GameData.Instance.GetPlayerById(id).Object.GetLpPlayer();
    public static List<LaunchpadPlayer> GetAllPlayers() => PlayerControl.AllPlayerControls.ToArray().Select(player => player.GetLpPlayer()).ToList();
    public static IEnumerable<LaunchpadPlayer> GetAllAlivePlayers() => GetAllPlayers().Where(plr => !plr.player.Data.IsDead && !plr.player.Data.Disconnected);
    private void Awake()
    {
        player = gameObject.GetComponent<PlayerControl>();
        votedPlayers = [];
        if (player.AmOwner)
        {
            LocalPlayer = this;
        }
    }

    public void OnDeath()
    {
        if (player.Data.IsHacked() && player.AmOwner)
        {
            player.RpcUnHackPlayer();
        }
    }


    private void FixedUpdate()
    {
        if (MeetingHud.Instance)
        {
            return;
        }

        if (player.IsRevived())
        {
            player.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.MedicColor));
        }

        if (player.Data.IsHacked())
        {
            var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6), "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
            player.cosmetics.SetName(randomString);
            player.cosmetics.SetNameMask(true);
            player.cosmetics.gameObject.SetActive(false);
        }

        if (knife is null)
        {
            knife = player.gameObject.transform.FindChild("BodyForms/Seeker/KnifeHand");
            return;
        }

        knife.gameObject.SetActive(!player.Data.IsDead && player.CanMove);
    }
}