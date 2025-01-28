using LaunchpadReloaded.Components;
using LaunchpadReloaded.Features;
using LaunchpadReloaded.Networking;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using System.Linq;
using LaunchpadReloaded.Modifiers;
using MiraAPI.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RegisterInIl2Cpp]
public class NodeMinigame(nint ptr) : Minigame(ptr)
{
    public Collider2D[] Sliders;
    private Controller myController = new Controller();
    public FloatRange SliderX = new FloatRange(-0.65f, 1.85f);
    private int sliderId;
    public TextMeshPro statusText;
    public TextMeshPro nodeIdText;
    private HackNodeComponent node;

    public void Open(HackNodeComponent node)
    {
        this.node = node;
        nodeIdText.text = $"node_{node.id}";

        statusText.text = "disabled";
        statusText.color = Color.red;
        Begin(null);
    }

    private void Awake()
    {
        DivertPowerMinigame miniGame = GetComponent<DivertPowerMinigame>();
        Sliders = miniGame.Sliders;
        OpenSound = miniGame.OpenSound;
        CloseSound = miniGame.CloseSound;
        miniGame.Destroy();

        sliderId = 0;

        var outsideBtn = transform.FindChild("BackgroundCloseButton/OutsideCloseButton").GetComponent<PassiveButton>();
        var closeBtn = transform.FindChild("CloseButton").GetComponent<ButtonBehavior>();
        statusText = transform.FindChild("StatusText").GetComponent<TextMeshPro>();
        nodeIdText = transform.FindChild("NodeId").GetComponent<TextMeshPro>();

        closeBtn.OnClick.AddListener((UnityAction)(() =>
        {
            Close();
        }));

        outsideBtn.OnClick.AddListener((UnityAction)(() =>
        {
            Close();
        }));

        for (int i = 0; i < this.Sliders.Length; i++)
        {
            if (i != this.sliderId)
            {
                this.Sliders[i].GetComponent<SpriteRenderer>().color = new Color(0, 0.5188679f, 0.1322604f);
            }
        }
    }

    private void FixedUpdate()
    {
        this.myController.Update();

        if (!node.isActive && amClosing == CloseState.None)
        {
            statusText.text = "enabled";
            statusText.color = Color.green;

            base.StartCoroutine(base.CoStartClose(0.6f));
            return;
        }

        if (amClosing != CloseState.None) return;


        if (sliderId == Sliders.Count()) return;

        Collider2D collider2D2 = this.Sliders[sliderId];
        Vector2 vector2 = collider2D2.transform.localPosition;
        DragState dragState = this.myController.CheckDrag(collider2D2);
        if (dragState == DragState.Dragging)
        {
            Vector2 vector3 = myController.DragPosition - (Vector2)collider2D2.transform.parent.position;
            vector3.x = this.SliderX.Clamp(vector3.x);
            vector2.x = vector3.x;
            collider2D2.transform.localPosition = vector2;
            return;
        }

        if (dragState != DragState.Released)
        {
            return;
        }

        if (this.SliderX.max - vector2.x < 0.05f)
        {
            sliderId += 1;
            collider2D2.GetComponent<SpriteRenderer>().color = new Color(0, 0.5188679f, 0.1322604f);

            SoundManager.Instance.PlaySoundImmediate(LaunchpadAssets.BeepSound.LoadAsset(), false, 0.8f);

            if (sliderId != Sliders.Count())
            {
                Sliders[sliderId].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0.2549479f);
            }
            else
            {
                statusText.text = "enabled";
                statusText.color = Color.green;

                base.StartCoroutine(base.CoStartClose(1f));
                PlayerControl.LocalPlayer.RpcRemoveModifier<HackedModifier>();

                return;
            }
        }
    }
}