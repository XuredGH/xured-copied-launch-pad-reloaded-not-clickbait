using LaunchpadReloaded.Options;
using MiraAPI.GameOptions;

namespace LaunchpadReloaded.Roles;
public interface ILaunchpadRole
{
    public bool CanSeeRoleTag()
    {
        return OptionGroupSingleton<GeneralOptions>.Instance.GhostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead;
    }
}
