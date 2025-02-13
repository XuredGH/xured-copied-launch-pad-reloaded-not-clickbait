using LaunchpadReloaded.Features;
using MiraAPI.Utilities;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LaunchpadReloaded.Components;

[RegisterInIl2Cpp]
public class PlayerTagManager(IntPtr ptr) : MonoBehaviour(ptr)
{
    public PlayerControl Player;

    public Transform TagHolder;
    public GameObject TagTemplate;
    public GridLayoutGroup GridLayout;

    public Dictionary<PlayerTag, GameObject> Tags;

    public void Awake()
    {
        Tags = new();
        Player = GetComponent<PlayerControl>();

        TagHolder = Instantiate(LaunchpadAssets.PlayerTags.LoadAsset(), Player.transform).transform;
        GridLayout = TagHolder.GetComponent<GridLayoutGroup>();

        TagTemplate = TagHolder.transform.GetChild(0).gameObject;
        TagTemplate.SetActive(false);
        TagTemplate.transform.SetParent(Player.transform); // Just to store it as a template

        UpdatePosition();
    }

    public void FixedUpdate()
    {
        foreach (var tagPair in Tags.Where(pair => pair.Value != null))
        {
            tagPair.Value.SetActive(tagPair.Key.IsLocallyVisible(Player) && Player.cosmetics.nameText.gameObject.active);
        }
    }

    public void UpdatePosition()
    {
        var colorblind = Player.cosmetics.colorBlindText.gameObject.active;
        var columnCount = Mathf.CeilToInt((float)GridLayout.transform.childCount / GridLayout.constraintCount) - 1;
        var nameTextY = (colorblind ? 1.1f : 0.91f) + (columnCount > 0 ? columnCount * 0.27f : 0);
        var holderY = 0.66f + (columnCount > 0 ? columnCount * 0.15f : 0);

        TagHolder.transform.localPosition = new Vector3(0, holderY, -0.35f);
        Player.cosmetics.nameTextContainer.transform.localPosition = new Vector3(0, nameTextY, -0.5f);
    }

    public PlayerTag? GetTagByName(string name)
    {
        if (Tags == null)
        {
            return null;
        }

        return Tags.Keys.FirstOrDefault(tag => tag.Name == name);
    }

    public void RemoveTag(PlayerTag plrTag)
    {
        if (!Tags.ContainsKey(plrTag))
        {
            return;
        }

        var tag = Tags[plrTag];
        tag.gameObject.DestroyImmediate();
        Tags.Remove(plrTag);

        UpdatePosition();
    }

    public void ClearTags()
    {
        foreach (var tag in Tags)
        {
            RemoveTag(tag.Key);
        }
    }

    public void AddTag(PlayerTag plrTag)
    {
        if (Tags.ContainsKey(plrTag))
        {
            return;
        }

        var newTag = Instantiate(TagTemplate, TagHolder.transform);
        var bgRend = newTag.GetComponent<SpriteRenderer>();
        var tagText = newTag.transform.GetChild(0).GetComponent<TextMeshPro>();
        tagText.text = plrTag.Text;
        newTag.name = plrTag.Name;

        if (plrTag.Color != Color.clear)
        {
            tagText.color = plrTag.Color.LightenColor(0.25f);
            bgRend.color = plrTag.Color;
        }

        Tags.Add(plrTag, newTag);

        UpdatePosition();
    }
}

public struct PlayerTag(string name, string text, Color color)
{
    public string Name { get; set; } = name;
    public string Text { get; set; } = text;
    public Color Color { get; set; } = color;
    public Func<PlayerControl, bool> IsLocallyVisible { get; set; }
}
