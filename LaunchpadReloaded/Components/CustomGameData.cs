using System;
using System.Collections.Generic;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Features.Managers;
using Reactor.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class CustomGameData(IntPtr ptr) : MonoBehaviour(ptr)
{
    public static CustomGameData? Instance { get; private set; } 
    
    public readonly List<CustomPlayerInfo> AllPlayers = [];

    public CustomPlayerInfo? GetPlayerById(byte id)
    {
        return AllPlayers.Find(x => x.Data.PlayerId == id);
    }

    public void AddPlayer(LaunchpadPlayer player)
    {
        var data = new CustomPlayerInfo(player);
        AllPlayers.Add(data);
    }
    
    private void Awake()
    {
        if (Instance)
        {
            Logger<LaunchpadReloadedPlugin>.Error("Custom Game Data already exists!");
            this.Destroy();
            return;
        }

        Instance = this;
    }


    public class CustomPlayerInfo
    {
        public NetworkedPlayerInfo Data { get; private set; }
        public LaunchpadPlayer Player { get; private set; }

        public int GradientColor;

        public bool GradientEnabled = true;
        
        public CustomPlayerInfo(LaunchpadPlayer pc)
        {
            Player = pc;
            Data = pc.playerObject.Data;

            if (Player.playerObject.AmOwner && AmongUsClient.Instance.NetworkMode is NetworkModes.FreePlay)
            {
                GradientColor = GradientManager.LocalGradientId;
            }
        }
    }
}