using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class TextMeshProTextAdapter : ITextDisplay
    {
        public TextMeshPro TextMesh { get; private set; }

        public TextMeshProTextAdapter(TextMeshPro textMesh)
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