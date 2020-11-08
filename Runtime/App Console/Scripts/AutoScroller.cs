using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private Button scrollButton;

    private float lastContentHeight;
    private bool scrollerActive;

    public bool ScrollerActive
    {
        get => scrollerActive;
        set
        {
            scrollerActive = value;
            scrollButton.gameObject.SetActive(!scrollerActive);
        }
    }


    private void Start()
    {
        lastContentHeight = content.sizeDelta.y;
        ScrollerActive = false;
    }

    private void OnEnable()
    {
        ScrollToBottom();
    }

    public void OnScrollValueChanged()
    {
        if (ScrollerActive)
        {
            bool scrolledManually = content.sizeDelta.y == lastContentHeight;
            if (scrolledManually)
            {
                ScrollerActive = false;
            }
            else
            {
                lastContentHeight = content.sizeDelta.y;
                ScrollToBottom();
            }
        }
    }

    public void ActivateScroller()
    {
        ScrollerActive = true;
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        if (ScrollerActive)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
