using Reactor.Utilities.Extensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LaunchpadReloaded.API.Options;

public abstract class CustomOption
{
    public abstract string Text { get; }
    public abstract string Id { get; }
    public bool Enabled = false;
    private GameObject newButton;

    public GameObject CreateButton(OptionsMenuBehaviour optionsMenu, Transform parent)
    {
        var generalTab = optionsMenu.transform.FindChild("GeneralTab");
        var button = generalTab.FindChild("ChatGroup").FindChild("CensorChatButton");
        newButton = GameObject.Instantiate(button, parent).gameObject;
        newButton.name = this.Id;

        var text = newButton.GetComponentInChildren<TextMeshPro>();
        text.text = Text;

        newButton.GetComponent<ToggleButtonBehaviour>().BaseText = StringNames.None;
        newButton.GetComponent<ToggleButtonBehaviour>().Rollover = newButton.GetComponent<ButtonRolloverHandler>();
        var pb = newButton.GetComponent<PassiveButton>();
        pb.OnClick.RemoveAllListeners();
        pb.OnClick.AddListener((UnityAction)OnClick);

        return newButton.gameObject;
    }

    public virtual void OnClick()
    {
        Enabled = !Enabled;
        newButton.GetComponent<ToggleButtonBehaviour>().onState = Enabled;
    }
}