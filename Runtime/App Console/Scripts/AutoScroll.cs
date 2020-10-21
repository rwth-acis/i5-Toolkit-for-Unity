using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Controls the scroll behaviour of the console
    /// </summary>
    public class AutoScroll : MonoBehaviour
    {
        [Tooltip("The scroll rect which determines which part of the console is visible")]
        [SerializeField] protected ScrollRect scrollRect;
        [Tooltip("Button which enables auto scrolling")]
        [SerializeField] protected Button autoScrollButton;
        [Tooltip("The text mesh which shows the content")]
        [SerializeField] protected TextMeshProUGUI content;

        private bool autoScroll;
        private bool updatePending;

        /// <summary>
        /// Set this value to true before changing the text content
        /// This way, it will not affect auto-scrolling
        /// </summary>
        public bool ExpectContentChange { get; set; }

        // adds event listeners for the scroll button and scroll rect
        private void Awake()
        {
            autoScrollButton.onClick.AddListener(OnAutoScrollButtonClicked);
            scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
        }

        // updates the auto scroll
        private void Update()
        {
            if (autoScroll && updatePending)
            {
                scrollRect.verticalNormalizedPosition = 0;
                updatePending = false;
            }
        }

        /// <summary>
        /// Called if the auto scroll button is clicked
        /// </summary>
        public void OnAutoScrollButtonClicked()
        {
            autoScroll = true;
            ApplyAutoscroll();
            autoScrollButton.gameObject.SetActive(!autoScroll);
        }

        /// <summary>
        /// Called if the scroll value of the connected UI is changed
        /// </summary>
        /// <param name="value"></param>
        public void OnScrollValueChanged(Vector2 value)
        {
            // if we expect a content change, this event was thrown by the changed text
            if (ExpectContentChange)
            {
                ExpectContentChange = false;
                ApplyAutoscroll();
            }
            else
            {
                autoScroll = value.y <= 0;
                autoScrollButton.gameObject.SetActive(!autoScroll);
            }
        }

        // sets the auto scroll update pending flag so that it can be picked up in the next update
        private void ApplyAutoscroll()
        {
            if (autoScroll)
            {
                updatePending = true;
            }
        }
    }
}