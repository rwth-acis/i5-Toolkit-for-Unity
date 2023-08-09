using i5.Toolkit.Core.Experimental.UnityAdapters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// MonoBehaviour for binding the auto scroller logic
    /// </summary>
    public class AutoScrollerBehaviour : MonoBehaviour
    {
        [Tooltip("The ScrollRect which should be scrolled")]
        [SerializeField] private ScrollRect scrollRect;
        [Tooltip("The content inside of the ScrollRect")]
        [SerializeField] private RectTransform content;
        [Tooltip("The button which triggers the automatic scrolling")]
        [SerializeField] private Button scrollButton;

        /// <summary>
        /// The instance which controls the auto scrolling
        /// </summary>
        public AutoScroller AutoScroller { get; private set; }

        // sets up the auto scroller
        private void Awake()
        {
            IScrollView scrollView = new ScrollRectAdapter(scrollRect);
            IRectangle rectangle = new RectTransformAdapter(content);
            IActivateable activateable = new GameObjectAdapter(scrollButton.gameObject);
            AutoScroller = new AutoScroller(scrollView, rectangle, activateable);
        }

        // enables the auto scroller
        private void OnEnable()
        {
            AutoScroller.OnEnable();
        }

		/// <summary>
		/// Called if the scroll value changes and notifies the auto scroller
		/// </summary>
		public void OnScrollValueChanged()
        {
            AutoScroller.NotifyScrollValueChanged();
        }

        /// <summary>
        /// Called if the button which activates the automatic scrolling is pressed
        /// </summary>
        public void OnAutoScrollButtonPressed()
        {
            AutoScroller.ScrollerActive = true;
        }
    }
}