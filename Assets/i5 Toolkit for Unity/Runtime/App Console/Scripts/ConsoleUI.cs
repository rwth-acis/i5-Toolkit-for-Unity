using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class ConsoleUI : MonoBehaviour, IMessageDisplayUI
{
    [SerializeField] private TextMeshProUGUI consoleTextDisplay;
    [SerializeField] private bool autoScroll;

    public void UpdateUI(List<string> messages)
    {
        string text = "";
        for (int i = 0; i < messages.Count; i++)
        {
            text += messages[i];
            if (i < messages.Count - 1)
            {
                text += Environment.NewLine;
            }
        }

        consoleTextDisplay.text = text;
    }
}