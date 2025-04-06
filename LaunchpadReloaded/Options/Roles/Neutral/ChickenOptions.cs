using LaunchpadReloaded.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace LaunchpadReloaded.Options.Roles.Neutral;

public class ChickenOptions : AbstractOptionGroup<ChickenRole>
{
    public override string GroupName => "Chicken";

    [ModdedToggleOption("Can Use Vents")]
    public bool CanUseVents { get; set; } = true;
}