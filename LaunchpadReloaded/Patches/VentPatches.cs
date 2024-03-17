using HarmonyLib;
using LaunchpadReloaded.API.GameModes;
using LaunchpadReloaded.API.Roles;
using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using UnityEngine;

namespace LaunchpadReloaded.Patches;

[HarmonyPatch(typeof(Vent))]
public static class VentPatches
{
    [HarmonyPrefix]
    [HarmonyPatch("CanUse")]
    public static bool CanUsePatch(Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
    {
        if (CustomGameModeManager.ActiveMode.CanVent(__instance, pc) == false)
        {
            return couldUse = canUse = false;
        }

        float num = float.MaxValue;
        PlayerControl @object = pc.Object;
        bool customRoleUsable = false;
        if (@object.Data.Role is ICustomRole role) customRoleUsable = role.CanUseVent;

        canUse = couldUse = (@object.inVent || customRoleUsable || CustomGameModeManager.ActiveMode.CanVent(__instance, pc))
            && !pc.IsDead && (@object.CanMove || @object.inVent);

        if (canUse)
        {
            Vector3 center = @object.Collider.bounds.center;
            Vector3 position = __instance.transform.position;
            num = Vector2.Distance(center, position);
            canUse &= (num <= __instance.UsableDistance && (!PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false) || __instance.name.StartsWith("JackInTheBoxVent_")));
        }
        __result = num;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch("SetOutline")]
    public static bool SetOutlinePatch(Vent __instance, [HarmonyArgument(0)] bool on, [HarmonyArgument(1)] bool mainTarget)
    {
        Color color = PlayerControl.LocalPlayer.Data.Role is ICustomRole role
            ? role.RoleColor : PlayerControl.LocalPlayer.Data.Role.IsImpostor ? Palette.ImpostorRed : Palette.CrewmateBlue;
        __instance.myRend.material.SetFloat("_Outline", (float)(on ? 1 : 0));
        __instance.myRend.material.SetColor("_OutlineColor", color);
        __instance.myRend.material.SetColor("_AddColor", mainTarget ? color : Color.clear);

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch("EnterVent")]
    [HarmonyPatch("ExitVent")]
    public static void EnterExitPostfix(Vent __instance)
    {
        var ventBody = __instance.GetComponent<VentBodyComponent>();
        if (ventBody && ventBody.deadBody)
        {
            DeadBodyManager.RpcExposeBody(ShipStatus.Instance, __instance.Id);
        }
    }
}