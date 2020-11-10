using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class AutoScrollerTests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

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

        [Test]
        public void NotifyOnScrollValueChanged_UserScrolled_DeactivatesScroller()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            // no changes to content size => manual scrolling
            autoScroller.NotifyOnScrollValueChanged();

            Assert.False(autoScroller.ScrollerActive);
        }

        [Test]
        public void NotifyOnScrollValueChanged_ContentSizeChanged_KeepsScrollerActive()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            content.Size = Vector2.one;

            // changes to content size => no manual scrolling
            autoScroller.NotifyOnScrollValueChanged();

            Assert.True(autoScroller.ScrollerActive);
        }

        [Test]
        public void NotifyOnScrollValueChanged_ContentSizeChanged_ScrollToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = true;

            content.Size = Vector2.one;
            scrollView.NormalizedPosition = Vector2.one;

            // changes to content size => no manual scrolling
            autoScroller.NotifyOnScrollValueChanged();

            Assert.AreEqual(0, scrollView.NormalizedPosition.y);
        }

        [Test]
        public void NotifyOnScrollValueChanged_ScrollerInactive_StaysDeactivated()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = false;

            autoScroller.NotifyOnScrollValueChanged();

            Assert.False(autoScroller.ScrollerActive);
        }

        [Test]
        public void NotifyOnScrollValueChanged_ScrollerInactiveContentSizeChanged_NoScrollToBottom()
        {
            AutoScroller autoScroller = CreateAutoScroller(
                out IScrollView scrollView,
                out IRectangle content,
                out IActivateable elementToStartScroller);

            autoScroller.ScrollerActive = false;

            content.Size = Vector2.one;
            scrollView.NormalizedPosition = Vector2.one;

            autoScroller.NotifyOnScrollValueChanged();

            Assert.AreEqual(1, scrollView.NormalizedPosition.y);
        }

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
