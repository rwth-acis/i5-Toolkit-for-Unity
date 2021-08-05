using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class TextMeshTextAdapter : ITextDisplay
    {
        public TextMesh TextMesh { get; private set; }

        public TextMeshTextAdapter(TextMesh textMesh)
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
