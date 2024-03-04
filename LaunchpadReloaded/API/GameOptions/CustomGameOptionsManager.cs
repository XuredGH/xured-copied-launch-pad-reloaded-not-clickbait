using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Rpc;
using Reactor.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.API.GameOptions;

public static class CustomGameOptionsManager
{
    public static readonly List<CustomGameOption> CustomOptions = new ();
    
    public static readonly List<CustomNumberOption> CustomNumberOptions = new();

    public static readonly List<CustomToggleOption> CustomToggleOptions = new();
    
    public static NumberOption NumberOptionPrefab { get; set; }
    public static ToggleOption ToggleOptionPrefab { get; set; }
    public static StringOption StringOptionPrefab { get; set; }
    
    public static void RpcSyncOptions()
    {
        var toggles = CustomToggleOptions.Select(x => x.Value).ToArray();
        var numbers = CustomNumberOptions.Select(x => x.Value).ToArray();
        Rpc<SyncCustomGameOptionsRpc>.Instance.Send(new SyncCustomGameOptionsRpc.Data(toggles, numbers));
    }
    
    public static void ReadSyncOptions(bool[] toggles, float[] numbers)
    {
        for (var i = 0; i < toggles.Length; i++)
        {
            CustomToggleOptions[i].SetValue(toggles[i]);
        }
        for (var i = 0; i < numbers.Length; i++)
        {
            CustomNumberOptions[i].SetValue(numbers[i]);
        }
    }
    
}