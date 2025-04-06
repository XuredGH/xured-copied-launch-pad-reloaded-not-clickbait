using LaunchpadReloaded.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace LaunchpadReloaded.Options.Roles.Impostor;

public class DevourerOptions : AbstractOptionGroup<DevourerRole>
{
    public override string GroupName => "Devourer";

    [ModdedNumberOption("Eat Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float EatCooldown { get; set; } = 25f;

    [ModdedNumberOption("Devoured Time", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float DevouredTime { get; set; } = 90f;
}