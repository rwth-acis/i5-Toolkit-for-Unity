using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public class TmpConsoleUIBehaviour : ConsoleUIBehaviour
    {
        [SerializeField] private TextMeshProUGUI consoleTextDisplay;
        [SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;

        protected override void Awake()
        {
            consoleUI = new TmpConsoleUI(consoleTextDisplay, logFormatterConfiguration);

            base.Awake();
        }
    }
}