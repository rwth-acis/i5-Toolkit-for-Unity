using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.UnityWrappers;
using NUnit.Framework;
using UnityEngine;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the AutoScroller component
    /// </summary>
    public class AutoScrollerTests
    {
        /// <summary>
        /// Resets the scene
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks if the interaction element which starts the scroller is deactiaved if the scroller if active
        /// </summary>
        [Test]
        public void ScrollerActive_SetTrue_DeactivatesElementToStartScroller()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            Assert.False(elementToStartScroller.ActiveSelf);
        }

        /// <summary>
        /// Checks if the scroller scrolls to be bottom if it is activated
        /// </summary>
        [Test]
        public void ScrollerActive_SetTrue_ScrollsToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            scrollView.NormalizedPosition = new Vector2(1, 1);

            autoScroller.ScrollerActive = true;

            Assert.AreEqual(0, scrollView.NormalizedPosition.y);
        }

        /// <summary>
        /// Checks if the interaction element which starts the scroller is activated if the scroller is not active
        /// </summary>
        [Test]
        public void ScrollerActive_SetFalse_ActivatesElementToStartScroller()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = false;

            Assert.True(elementToStartScroller.ActiveSelf);
        }

        /// <summary>
        /// Checks if the scroller scrolls to the bottom when OnEnable is called
        /// </summary>
        [Test]
        public void OnEnable_ScrollerActive_ScrollsToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            scrollView.NormalizedPosition = new Vector2(1, 1);

            autoScroller.OnEnable();

            Assert.AreEqual(0, scrollView.NormalizedPosition.y);
        }

        /// <summary>
        /// Checks that the scroller is deactivated if the user scrolls the scrollview
        /// </summary>
        [Test]
        public void NotifyScrollValueChanged_UserScrolled_DeactivatesScroller()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            // no changes to content size => manual scrolling
            autoScroller.NotifyScrollValueChanged();

            Assert.False(autoScroller.ScrollerActive);
        }

        /// <summary>
        /// Checks that the scroller stays active if the scroll value changes because of a content update
        /// </summary>
        [Test]
        public void NotifyScrollValueChanged_ContentSizeChanged_KeepsScrollerActive()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            content.Size = Vector2.one;

            // changes to content size => no manual scrolling
            autoScroller.NotifyScrollValueChanged();

            Assert.True(autoScroller.ScrollerActive);
        }

        /// <summary>
        /// Checks that the scroller scrolls to the bottom if the content size was changed and the scroller is active
        /// </summary>
        [Test]
        public void NotifyScrollValueChanged_ContentSizeChanged_ScrollToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            content.Size = Vector2.one;
            scrollView.NormalizedPosition = Vector2.one;

            // changes to content size => no manual scrolling
            autoScroller.NotifyScrollValueChanged();

            Assert.AreEqual(0, scrollView.NormalizedPosition.y);
        }

        /// <summary>
        /// Checks that the scroller stays deactivated if the scroll value changes and the scroller is inactive
        /// </summary>
        [Test]
        public void NotifyScrollValueChanged_ScrollerInactive_StaysDeactivated()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = false;

            autoScroller.NotifyScrollValueChanged();

            Assert.False(autoScroller.ScrollerActive);
        }

        /// <summary>
        /// Checks that the scroller does not scroll to bottom if it is deactivated and scroll value changes because of an content size change
        /// </summary>
        [Test]
        public void NotifyScrollValueChanged_ScrollerInactiveContentSizeChanged_NoScrollToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = false;

            content.Size = Vector2.one;
            scrollView.NormalizedPosition = Vector2.one;

            autoScroller.NotifyScrollValueChanged();

            Assert.AreEqual(1, scrollView.NormalizedPosition.y);
        }

        // creates an auto scroller object for the tests
        private AutoScroller CreateAutoScroller(out IScrollView scrollView, out IRectangle content, out IActivateable elementToStartScroller)
        {
            scrollView = A.Fake<IScrollView>();
            content = A.Fake<IRectangle>();
            elementToStartScroller = A.Fake<IActivateable>();
            AutoScroller autoScroller = new AutoScroller(scrollView, content, elementToStartScroller);
            return autoScroller;
        }
    }
}
