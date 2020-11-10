using i5.Toolkit.Core.Utilities.UnityAdapters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace i5.Toolkit.Core.AppConsole
{
    public class AutoScrollerBehaviour : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform content;
        [SerializeField] private Button scrollButton;

        public AutoScroller AutoScroller { get; private set; }

        private void Awake()
        {
            IScrollView scrollView = new ScrollRectAdapter(scrollRect);
            IRectangle rectangle = new RectTransformAdapter(content);
            IActivateable activateable = new GameObjectAdapter(scrollButton.gameObject);
            AutoScroller = new AutoScroller(scrollView, rectangle, activateable);
        }

        private void OnEnable()
        {
            AutoScroller.OnEnable();
        }

        public void OnScrollValueChanged()
        {
            AutoScroller.NotifyOnScrollValueChanged();
        }

        public void OnAutoScrollButtonPressed()
        {
            AutoScroller.ScrollerActive = true;
        }
    }
}