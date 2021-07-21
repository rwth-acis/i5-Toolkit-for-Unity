using i5.Toolkit.Core.Experimental.UnityAdapters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Auto scroller which is automatically scrolls a scrollview to the bottom
    /// The scroll view can be activated by an UI element and deactivated by manually scrolling the view
    /// </summary>
    public class AutoScroller
    {
        private IScrollView scrollView;
        private IRectangle content;
        private IActivateable elementToStartScroller;

        private float lastContentHeight;
        private bool scrollerActive;

        /// <summary>
        /// If true, the scroller will make sure that the scrollview is always scrolled to the bottom
        /// </summary>
        public bool ScrollerActive
        {
            get => scrollerActive;
            set
            {
                scrollerActive = value;
                elementToStartScroller.ActiveSelf = !scrollerActive;
                if (scrollerActive)
                {
                    lastContentHeight = content.Size.y;
                    ScrollToBottom();
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the auto scroller
        /// </summary>
        /// <param name="scrollView">The view which should be scrolled</param>
        /// <param name="content">The content rectangle inside of the scrollview</param>
        /// <param name="elementToStartScroller">The interaction element which starts the scroller</param>
        public AutoScroller(IScrollView scrollView, IRectangle content, IActivateable elementToStartScroller)
        {
            this.scrollView = scrollView;
            this.content = content;
            this.elementToStartScroller = elementToStartScroller;

            lastContentHeight = content.Size.y;
            ScrollerActive = false;
        }

        /// <summary>
        /// Scrolls the scrollview to the bottom when the object is enabled
        /// </summary>
        public void OnEnable()
        {
            if (scrollerActive)
            {
                ScrollToBottom();
            }
        }

        /// <summary>
        /// Notifies the scroller that the scroll view was changed
        /// This can be due to two reasons:
        /// 1. The user scrolled the UI element
        /// 2. The size of the scroll view's content changed
        /// </summary>
        public void NotifyScrollValueChanged()
        {
            if (ScrollerActive)
            {
                bool scrolledManually = content.Size.y == lastContentHeight;
                if (scrolledManually)
                {
                    ScrollerActive = false;
                }
                else
                {
                    lastContentHeight = content.Size.y;
                    ScrollToBottom();
                }
            }
        }

        // scrolls the scroll view to the bottom
        private void ScrollToBottom()
        {
            if (ScrollerActive)
            {
                scrollView.NormalizedPosition = new Vector2(scrollView.NormalizedPosition.x, 0f);
            }
        }
    }
}