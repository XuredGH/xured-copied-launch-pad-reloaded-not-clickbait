using System.Collections.Generic;
using System.Linq;
using LaunchpadReloaded.Networking;
using Reactor.Networking.Rpc;

namespace LaunchpadReloaded.API.GameOptions;

public static class CustomGameOptionsManager
{
    public static readonly List<CustomOption> CustomOptions = new ();
    
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
    
    public static CustomNumberOption CreateNumberOption(string title, float defaultValue, float minValue, float maxValue, float increment, NumberSuffixes suffixType)
    {
        return CustomNumberOptions.FirstOrDefault(x => x.Title == title) ?? new CustomNumberOption(title, defaultValue, minValue, maxValue, increment, suffixType);
    }

    public static CustomToggleOption CreateToggleOption(string title, bool defaultValue)
    {
        return CustomToggleOptions.FirstOrDefault(x => x.Title == title) ?? new CustomToggleOption(title, defaultValue);
    }
    
}