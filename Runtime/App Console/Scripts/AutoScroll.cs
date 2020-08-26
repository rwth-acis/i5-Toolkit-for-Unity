using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected Button autoScrollButton;
    [SerializeField] protected TextMeshProUGUI content;

    private bool autoScroll;

    public bool ExpectContentChange { get; set; }

    private void Awake()
    {
        autoScrollButton.onClick.AddListener(OnAutoScrollButtonClicked);
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void OnAutoScrollButtonClicked()
    {
        autoScroll = true;
        ApplyAutoscroll();
    }

    public void OnScrollValueChanged(Vector2 value)
    {
        if (ExpectContentChange)
        {
            ExpectContentChange = false;
            ApplyAutoscroll();
        }
        else
        {
            autoScroll = value.y <= 0;
        }
    }


    private void ApplyAutoscroll()
    {
        if (autoScroll)
        {
            content.ForceMeshUpdate();
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}
