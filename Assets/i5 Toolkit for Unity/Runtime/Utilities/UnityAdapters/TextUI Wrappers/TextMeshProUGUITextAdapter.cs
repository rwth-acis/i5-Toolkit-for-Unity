using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace i5.Toolkit.Core.Utilities.UnityAdapters
{
    public class TextMeshProUGUITextAdapter : ITextDisplay
    {
        public TextMeshProUGUI TextMesh { get; private set; }

        public TextMeshProUGUITextAdapter(TextMeshProUGUI textMesh)
        {
            TextMesh = textMesh;
        }

        public string Text
        {
            get => TextMesh.text;
            set => TextMesh.text = value;
        }
    }
}