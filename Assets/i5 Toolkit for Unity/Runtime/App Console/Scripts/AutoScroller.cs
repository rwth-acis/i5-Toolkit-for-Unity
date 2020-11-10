using i5.Toolkit.Core.Utilities.UnityAdapters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.AppConsole
{
    public class AutoScroller
    {
        private IScrollView scrollView;
        private IRectangle content;
        private IActivateable elementToStartScroller;

        private float lastContentHeight;
        private bool scrollerActive;

        public bool ScrollerActive
        {
            get => scrollerActive;
            set
            {
                scrollerActive = value;
                elementToStartScroller.ActiveSelf = !scrollerActive;
                if (scrollerActive)
                {
                    ScrollToBottom();
                }
            }
        }

        public AutoScroller(IScrollView scrollView, IRectangle content, IActivateable elementToStartScroller)
        {
            this.scrollView = scrollView;
            this.content = content;
            this.elementToStartScroller = elementToStartScroller;

            lastContentHeight = content.Size.y;
            ScrollerActive = false;
        }

        public void OnEnable()
        {
            ScrollToBottom();
        }

        public void NotifyOnScrollValueChanged()
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

        private void ScrollToBottom()
        {
            if (ScrollerActive)
            {
                scrollView.NormalizedPosition = new Vector2(scrollView.NormalizedPosition.x, 0f);
            }
        }
    }
}