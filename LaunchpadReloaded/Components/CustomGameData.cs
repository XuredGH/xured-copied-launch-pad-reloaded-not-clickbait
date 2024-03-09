using System;
using System.Collections.Generic;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class CustomGameData(IntPtr ptr) : MonoBehaviour(ptr)
{
    public GameData gameData;

    public readonly Dictionary<byte,CustomPlayerInfo> CustomPlayerInfos = new ();
    public void Start()
    {
        gameData = GetComponent<GameData>();
    }
    
    public CustomPlayerInfo GetCustomInfo(byte playerId)
    {
        return CustomPlayerInfos[playerId];
    }

    public class CustomPlayerInfo
    {
        public byte PlayerId;
        public int SecondColorId;

        public CustomPlayerInfo(byte playerId, int secondColorId)
        {
            PlayerId = playerId;
            SecondColorId = secondColorId;
        }
    }
}