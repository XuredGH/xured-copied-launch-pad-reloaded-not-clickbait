using HarmonyLib;
using LaunchpadReloaded.Utilities;
using UnityEngine;

namespace LaunchpadReloaded.Patches.ColorScroll;
[HarmonyPatch(typeof(PlayerTab))]
public static class ScrollingColorsPatch
{
    /// Collider
    private static BoxCollider2D _collider = null;

    /// <summary>
    /// Add scrolling to the colors tab
    /// </summary>
    [HarmonyPostfix, HarmonyPatch("OnEnable")]
    public static void AddScrollingToColorsTabPatch(PlayerTab __instance)
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

            _collider = maskObj.AddComponent<BoxCollider2D>();
            _collider.size = new Vector2(1f, 0.75f);
            _collider.enabled = true;
            __instance.scroller = newScroller;
        }

        foreach (ColorChip chip in __instance.ColorChips)
        {
            chip.transform.SetParent(__instance.scroller.Inner);
            chip.Button.ClickMask = _collider;
            chip.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        __instance.SetScrollerBounds();
    }
}