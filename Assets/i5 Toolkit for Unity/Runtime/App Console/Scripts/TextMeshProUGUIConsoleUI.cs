using i5.Toolkit.Core.Experimental.UnityAdapters;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// MonoBehaviour for constructing console UIs with the TextMeshProUGUI
    /// </summary>
    public class TextMeshProUGUIConsoleUI : ConsoleUIBehaviour
    {
        [Tooltip("The text display which should show the messages")]
        [SerializeField] private TextMeshProUGUI consoleTextDisplay;
        [Tooltip("The formatter configuration which defines how messages are formatted")]
        [SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;

        // initializes the text console UI
        protected override void Awake()
        {
            ITextDisplay textMeshProUGUIAdapter = new TextMeshProUGUITextAdapter(consoleTextDisplay);
            consoleUI = new TextConsoleUI(textMeshProUGUIAdapter, logFormatterConfiguration);

            base.Awake();
        }
    }
}