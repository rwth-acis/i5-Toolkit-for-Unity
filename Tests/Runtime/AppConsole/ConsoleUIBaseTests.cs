using System.Collections;
using System.Collections.Generic;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    public class ConsoleUIBaseTests
    {
        [SetUp]
        public void SetUp()
        {
            PlayModeTestUtilities.LoadEmptyTestScene();
        }

        [Test]
        public void OnEnable_ActivatesConsoleCapture()
        {
            GameObject go = new GameObject();
            ConsoleUIBehaviour consoleUIBase = go.AddComponent<ConsoleUIBehaviour>();
        }
    }
}
