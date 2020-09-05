using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditModeTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }
    }
}
