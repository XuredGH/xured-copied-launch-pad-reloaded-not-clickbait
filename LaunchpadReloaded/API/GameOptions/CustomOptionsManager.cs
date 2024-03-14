using Reactor.Networking.Rpc;
using System.Collections.Generic;
using System.Linq;

namespace LaunchpadReloaded.API.GameOptions;

public static class CustomOptionsManager
{
    public static readonly List<AbstractGameOption> CustomOptions = new();
    public static readonly List<CustomNumberOption> CustomNumberOptions = new();
    public static readonly List<CustomToggleOption> CustomToggleOptions = new();
    public static readonly List<CustomStringOption> CustomStringOptions = new();
    public static readonly List<CustomOptionGroup> CustomGroups = new();
    public static void SyncOptions()
    {
        var toggles = CustomToggleOptions.Select(x => x.Value).ToArray();
        var numbers = CustomNumberOptions.Select(x => x.Value).ToArray();
        var strings = CustomStringOptions.Select(x => x.Value).ToArray();

        Rpc<SyncOptionsRpc>.Instance.Send(new SyncOptionsRpc.Data(toggles, numbers, strings));
    }

    public static void ResetToDefault()
    {
        foreach (CustomNumberOption numberOpt in CustomNumberOptions) numberOpt.SetValue(numberOpt.Default);
        foreach (CustomStringOption stringOpt in CustomStringOptions) stringOpt.SetValue(stringOpt.Default);
        foreach (CustomToggleOption toggleOpt in CustomToggleOptions) toggleOpt.SetValue(toggleOpt.Default);
        foreach (AbstractGameOption option in CustomOptions) option.ValueChanged(option.OptionBehaviour);
    }

    public static void HandleOptionsSync(bool[] toggles, float[] numbers, string[] strings)
    {
        for (var i = 0; i < toggles.Length; i++)
        {
            CustomToggleOptions[i].SetValue(toggles[i]);
        }
        for (var i = 0; i < numbers.Length; i++)
        {
            CustomNumberOptions[i].SetValue(numbers[i]);
        }
        for (var i = 0; i < strings.Length; i++)
        {
            CustomStringOptions[i].SetValue(strings[i]);
        }
    }

}