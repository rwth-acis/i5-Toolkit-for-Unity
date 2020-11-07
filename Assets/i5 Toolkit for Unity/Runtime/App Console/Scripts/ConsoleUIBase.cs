using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.AppConsole
{
    public abstract class ConsoleUIBase : MonoBehaviour
    {
        [SerializeField] protected LogFormatterConfiguration logFormatterConfiguration;
        [SerializeField] protected AutoScroll autoScroll;
        [SerializeField] protected bool captureInBackground;

        protected ILogFormatter logFormatter;

        protected Console console;

        protected virtual void Awake()
        {
            if (logFormatterConfiguration == null)
            {
                logFormatter = new DefaultConsoleFormatter();
            }
            else
            {
                logFormatter = logFormatterConfiguration.GenerateFormatter();
            }
            console = new Console();
        }

        protected virtual void OnEnable()
        {
            console.IsCapturing = true;
            console.OnConsoleContentChanged += Console_OnConsoleContentChanged;
            UpdateUI();
        }

        protected virtual void OnDisable()
        {
            if (!captureInBackground)
            {
                console.IsCapturing = false;
            }
            console.OnConsoleContentChanged -= Console_OnConsoleContentChanged;
        }

        private void Console_OnConsoleContentChanged()
        {
            UpdateUI();
        }

        protected virtual void UpdateUI()
        {
            autoScroll.ExpectContentChange = true;
        }
    }

}