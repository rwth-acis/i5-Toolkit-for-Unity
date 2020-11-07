using i5.Toolkit.Core.Utilities.UnityAdapters;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class TextMeshProUGUIConsoleUI : ConsoleUIBehaviour
    {
        [SerializeField] private TextMeshProUGUI consoleTextDisplay;
        [SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;

        protected override void Awake()
        {
            ITextAdapter textMeshProUGUIAdapter = new TextMeshProUGUITextAdapter(consoleTextDisplay);
            consoleUI = new TextConsoleUI(textMeshProUGUIAdapter, logFormatterConfiguration);

            base.Awake();
        }
    }
}