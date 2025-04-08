using LaunchpadReloaded.Features;
using MiraAPI.Roles;
using System;
using TMPro;
using UnityEngine;

namespace LaunchpadReloaded.Roles.Crewmate;

public class ChameleonRole(IntPtr ptr) : CrewmateRole(ptr), ICustomRole
{
    public string RoleName => "Chameleon";
    public string RoleDescription => "Become invisible when not moving.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => LaunchpadPalette.ChameleonColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.FreezeButton,
        OptionsScreenshot = LaunchpadAssets.CaptainBanner,
    };

    public void PlayerControlFixedUpdate(PlayerControl playerControl)
    {
        if (playerControl.MyPhysics.Velocity.magnitude > 0)
        {
            SpriteRenderer rend = playerControl.cosmetics.currentBodySprite.BodySprite;
            TextMeshPro tmp = playerControl.cosmetics.nameText;
            tmp.color = Color.Lerp(tmp.color, new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1), Time.deltaTime * 4f);
            rend.color = Color.Lerp(rend.color, new Color(1, 1, 1, 1), Time.deltaTime * 4f);

            GameObject pet = playerControl.cosmetics.currentPet.gameObject;
            SpriteRenderer petsprite = pet.GetComponent<SpriteRenderer>();
            Color originalColor = petsprite.color;
            float targetAlpha = 1f;
            float fadeSpeed = 4f;
            float newAlpha = Mathf.Lerp(originalColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
            petsprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            foreach (var cosmetic in playerControl.cosmetics.transform.GetComponentsInChildren<SpriteRenderer>())
            {
                cosmetic.color = Color.Lerp(cosmetic.color, new Color(1, 1, 1, 1), Time.deltaTime * 4f);
            }
        }
        else
        {
            SpriteRenderer rend = playerControl.cosmetics.currentBodySprite.BodySprite;
            TextMeshPro tmp = playerControl.cosmetics.nameText;
            tmp.color = Color.Lerp(tmp.color, new Color(tmp.color.r, tmp.color.g, tmp.color.b, playerControl.AmOwner ? 0.3f : 0), Time.deltaTime * 4f);
            rend.color = Color.Lerp(rend.color, new Color(1, 1, 1, playerControl.AmOwner ? 0.3f : 0), Time.deltaTime * 4f);

            GameObject pet = playerControl.cosmetics.currentPet.gameObject;
            SpriteRenderer petsprite = pet.GetComponent<SpriteRenderer>();
            Color originalColor = petsprite.color;
            float targetAlpha = 0.3f;
            float fadeSpeed = 4f;
            float newAlpha = Mathf.Lerp(originalColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
            petsprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            


            foreach (var cosmetic in playerControl.cosmetics.transform.GetComponentsInChildren<SpriteRenderer>())
            {
                cosmetic.color = Color.Lerp(cosmetic.color, new Color(1, 1, 1, playerControl.AmOwner ? 0.3f : 0), Time.deltaTime * 4f);
            }
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        Deinitialize(Player);
    }

    public override void Deinitialize(PlayerControl playerControl)
    {
        if (!playerControl.AmOwner)
        {
            return;
        }

        GameObject pet = playerControl.cosmetics.currentPet.gameObject;
        SpriteRenderer petsprite = pet.GetComponent<SpriteRenderer>();
        Color originalColor = petsprite.color;
        petsprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

    }

}