using LaunchpadReloaded.Features;
using MiraAPI.Utilities;
using MonoMod.Utils;
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
    public PlayerVoteArea? VoteArea;

    public Transform TagHolder;
    public GameObject TagTemplate;
    public GridLayoutGroup GridLayout;

    public Transform MeetingTagHolder;

    public Dictionary<PlayerTag, GameObject> Tags;
    private Dictionary<PlayerTag, GameObject> _oldTags;

    private bool _inMeeting = false;

    public void Awake()
    {
        Tags = new();
        _oldTags = new();
        Player = GetComponent<PlayerControl>();

        TagHolder = Instantiate(LaunchpadAssets.PlayerTags.LoadAsset(), Player.transform).transform;
        GridLayout = TagHolder.GetComponent<GridLayoutGroup>();

        TagTemplate = TagHolder.transform.GetChild(0).gameObject;
        TagTemplate.SetActive(false);
        TagTemplate.transform.SetParent(Player.transform); // Just to store it as a template

        UpdatePosition();
    }

    public void MeetingStart()
    {
        var meeting = MeetingHud.Instance;
        if (meeting != null)
        {
            _inMeeting = true;

            VoteArea = meeting.playerStates.FirstOrDefault(plr => plr.TargetPlayerId == Player.PlayerId);

            if (VoteArea != null)
            {
                MeetingTagHolder = Instantiate(LaunchpadAssets.PlayerTags.LoadAsset(), VoteArea.transform).transform;
                MeetingTagHolder.transform.GetChild(0).gameObject.DestroyImmediate();
                MeetingTagHolder.gameObject.layer = VoteArea.gameObject.layer;

                Dictionary<PlayerTag, GameObject> toAdd = new();

                foreach (var tagPair in Tags)
                {
                    _oldTags.Add(tagPair.Key, tagPair.Value);

                    var cloneTag = Instantiate(tagPair.Value);
                    cloneTag.transform.SetParent(MeetingTagHolder, false);
                    cloneTag.layer = MeetingTagHolder.gameObject.layer;
                    cloneTag.transform.GetChild(0).gameObject.layer = cloneTag.layer;

                    toAdd.Add(tagPair.Key, cloneTag);
                }

                Tags.Clear();
                Tags.AddRange(toAdd);

                MeetingTagHolder.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                UpdatePosition();
            }
        }
    }

    public void MeetingEnd()
    {
        _inMeeting = false;

        Tags.Clear();

        foreach (var tagPair in _oldTags)
        {
            Tags[tagPair.Key] = tagPair.Value;
        }

        _oldTags.Clear();

        UpdatePosition();
    }

    public void Update()
    {
        foreach (var tagPair in Tags)
        {
            if (tagPair.Value == null)
            {
                continue;
            }

            bool visible = tagPair.Key.IsLocallyVisible(Player)
                && (_inMeeting ? VoteArea!.NameText.gameObject.active : Player.cosmetics.nameText.gameObject.active);

            if (tagPair.Value.active == false && visible)
            {
                UpdatePosition();
            }

            tagPair.Value.SetActive(visible);
        }
    }

    public int GetActiveCount()
    {
        return Tags.Keys.Where(obj => obj.IsLocallyVisible(Player)).Count();
    }

    public void UpdatePosition()
    {
        var columnCount = Mathf.CeilToInt((float)GetActiveCount() / GridLayout.constraintCount);

        if (columnCount <= 0)
        {
            return;
        }

        var colorblind = Player.cosmetics.colorBlindText.gameObject.active;
        var nameTextY = (colorblind ? 0.8f : 0.65f) + (columnCount > 0 ? columnCount * 0.27f : 0);
        var holderY = 0.53f + (columnCount > 0 ? columnCount * 0.15f : 0);

        if (_inMeeting)
        {
            var nameTextPos = VoteArea!.NameText.transform.localPosition;

            holderY = -0.07f + (columnCount > 0 ? columnCount * -0.05f : 0);
            nameTextY = 0.025f + (columnCount > 0 ? columnCount * 0.1f : 0);

            VoteArea!.ColorBlindName.transform.localPosition = new Vector3(-0.9058f, -0.1666f, -0.01f);
            VoteArea!.NameText.transform.localPosition = new Vector3(nameTextPos.x, nameTextY, nameTextPos.z);
            MeetingTagHolder.transform.localPosition = new Vector3(nameTextPos.x, holderY, 0);
        }
        else
        {
            TagHolder.transform.localPosition = new Vector3(0, holderY, -0.35f);
            Player.cosmetics.nameTextContainer.transform.localPosition = new Vector3(0, nameTextY, -0.5f);
        }
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
