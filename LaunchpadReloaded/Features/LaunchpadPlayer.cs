using LaunchpadReloaded.Features.Managers;
using LaunchpadReloaded.Roles;
using LaunchpadReloaded.Utilities;
using Reactor.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaunchpadReloaded.Features;

[RegisterInIl2Cpp]
public class LaunchpadPlayer(IntPtr ptr) : MonoBehaviour(ptr)
{
    public List<byte> VotedPlayers;
    public int VotesRemaining;
    public bool DidSkip;

    public Transform Knife;

    public PlayerControl Player;
    public static LaunchpadPlayer LocalPlayer;
    public static LaunchpadPlayer GetById(byte id) => GameData.Instance.GetPlayerById(id).Object.GetLpPlayer();
    public static List<LaunchpadPlayer> GetAllPlayers() => PlayerControl.AllPlayerControls.ToArray().Select((player) => player.GetLpPlayer()).ToList();
    public static List<LaunchpadPlayer> GetAllAlivePlayers() => GetAllPlayers().Where((plr) => !plr.Player.Data.IsDead && !plr.Player.Data.Disconnected).ToList();
    private void Awake()
    {
        Player = gameObject.GetComponent<PlayerControl>();
        VotedPlayers = new List<byte>();
        if (Player.AmOwner) LocalPlayer = this;
    }

    public void OnDeath()
    {
        if (Player.Data.IsHacked())
        {
            HackingManager.RpcUnHackPlayer(Player);
        }
    }


    private void FixedUpdate()
    {
        if (MeetingHud.Instance) return;
        if (Player.IsRevived()) Player.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.MedicColor));

        if (Player.Data.IsHacked() || (HackingManager.Instance && HackingManager.Instance.AnyActiveNodes() && Player.Data.Role is HackerRole))
        {
            var randomString = Helpers.RandomString(Helpers.Random.Next(4, 6), "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000");
            Player.cosmetics.SetName(randomString);
            Player.cosmetics.SetNameMask(true);
            Player.cosmetics.gameObject.SetActive(false);
        }

        if (Knife is null)
        {
            Knife = Player.gameObject.transform.FindChild("BodyForms/Seeker/KnifeHand");
            return;
        }

        Knife.gameObject.SetActive(!Player.Data.IsDead && Player.CanMove);
    }
}