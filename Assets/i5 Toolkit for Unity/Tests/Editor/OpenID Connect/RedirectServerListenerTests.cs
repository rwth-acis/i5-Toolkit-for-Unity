using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.TestUtilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.OpenIDConnectClient
{
    public class RedirectServerListenerTests
    {
        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        //[Test]
        //public void StartServer_ServerOffline_ServerActiveTrue()
        //{
        //    RedirectServerListener rsl = new RedirectServerListener();

        //    rsl.StartServer();

        //    Assert.IsTrue(rsl.ServerActive);
        //}

        [Test]
        public void StartServer_ServerAlreadyRunning_WarningLogged()
        {
            RedirectServerListener rsl = new RedirectServerListener();
            rsl.StartServer();

            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Server is already running\w*"));

            rsl.StartServer();
        }

        [Test]
        public void StopServerImmediately_ServerOffline_WarningLogged()
        {
            RedirectServerListener rsl = new RedirectServerListener();

            LogAssert.Expect(LogType.Warning, new Regex(@"\w*Server is already stopped\w*"));

            rsl.StopServerImmediately();
        }

        [Test]
        public void StopServerImmediately_ServerRunning_ServerActiveFalse()
        {
            RedirectServerListener rsl = new RedirectServerListener();
            rsl.StartServer();

            rsl.StopServerImmediately();
            Assert.IsFalse(rsl.ServerActive);
        }

        [Test]
        public void GenerateRedirectUri_DefaultSchema_StoresRedirectUri()
        {
            RedirectServerListener rsl = new RedirectServerListener();

            string generatedUri = rsl.GenerateListeningUri();

            Assert.AreEqual(generatedUri, rsl.ListeningUri);
        }

        [Test]
        public void GenerateRedirectUri_DefaultSchema_UsesHttpSchema()
        {
            RedirectServerListener rsl = new RedirectServerListener();

            string generatedUri = rsl.GenerateListeningUri();

            Assert.IsTrue(generatedUri.StartsWith("http://"));
        }

        [Test]
        public void GenerateRedirectUri_CustomSchema_UsesCustomSchema()
        {
            RedirectServerListener rsl = new RedirectServerListener();

            string generatedUri = rsl.GenerateListeningUri("test");

            Assert.IsTrue(generatedUri.StartsWith("test://"));
        }

        [Test]
        public void GenerateRedirectUri_DefaultSchema_ContainsPort()
        {
            RedirectServerListener rsl = new RedirectServerListener();

            string generatedUri = rsl.GenerateListeningUri();

            generatedUri = generatedUri.Replace("http://", "");

            Assert.IsTrue(generatedUri.Contains(":"));
        }
    }
}
