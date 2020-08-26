using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ConsoleUIBase : MonoBehaviour
{
    [SerializeField] protected ConsoleFormatterBase consoleFormatter;
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected Button autoScrollButton;

    private bool autoScroll;

    protected virtual void Awake()
    {
        if (consoleFormatter == null)
        {
            consoleFormatter = new DefaultConsoleFormatter();
        }
        autoScrollButton.onClick.AddListener(OnAutoScrollButtonClicked);
    }

    public virtual void UpdateUI(List<INotificationMessage> notificationMessages)
    {
        ApplyAutoscroll();
    }

    public virtual void OnScrollValueChanged(Vector2 value)
    {
        //if (!scrollLock && value.y < 1f)
        //{
        //    autoScroll = false;
        //    Debug.Log("no auto scroll anymore");
        //}
    }

    public virtual void OnAutoScrollButtonClicked()
    {
        autoScroll = true;
        ApplyAutoscroll();
    }

    private void ApplyAutoscroll()
    {
        if (autoScroll)
        {
            StartCoroutine(AutoscrollAfterTMPUpdate());
        }
    }

    private IEnumerator AutoscrollAfterTMPUpdate()
    {
        // wait one frame so that the TMP can update
        yield return null;
        scrollRect.verticalNormalizedPosition = 0;
    }
}

