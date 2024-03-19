using HarmonyLib;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches.ColorScroll;
[HarmonyPatch]
public static class ScrollingColorsPatch
{
    public static BoxCollider2D coll = null;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public static void CreateCustomTab(PlayerTab __instance)
    {
        if (PlayerCustomizationMenu.Instance is null) return;
        InventoryTab tab = PlayerCustomizationMenu.Instance.Tabs[1].Tab;

        if (__instance.scroller == null)
        {
            Scroller newScroller = GameObject.Instantiate(tab.scroller);
            newScroller.Inner.transform.DestroyChildren();
            newScroller.transform.SetParent(__instance.transform);

            GameObject maskObj = new GameObject();
            maskObj.layer = 5;
            maskObj.name = "SpriteMask";
            maskObj.transform.SetParent(__instance.transform);
            maskObj.transform.localPosition = new Vector3(0, 0, 0);
            maskObj.transform.localScale = new Vector3(500, 4.76f);

            SpriteMask mask = maskObj.AddComponent<SpriteMask>();
            mask.sprite = LaunchpadAssets.BlankButton.LoadAsset();

            coll = maskObj.AddComponent<BoxCollider2D>();
            coll.size = new Vector2(1f, 0.75f);
            coll.enabled = true;
            __instance.scroller = newScroller;
        }

        foreach (ColorChip chip in __instance.ColorChips)
        {
            chip.transform.SetParent(__instance.scroller.Inner);
            chip.Button.ClickMask = coll;
            chip.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        __instance.SetScrollerBounds();
    }
}