﻿using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.TestHelpers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.OpenIDConnectClient
{
    public class OpenIDConnectServiceTests
    {
        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void ServerListener_DefaultNotNull()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            Assert.NotNull(oidc.ServerListener);
        }

        [Test]
        public void Cleanup_ServerRunning_StopsServer()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            A.CallTo(() => serverListener.ServerActive).Returns(true);
            oidc.ServerListener = serverListener;

            oidc.Cleanup();

            A.CallTo(() => serverListener.StopServerImmediately()).MustHaveHappened();
        }

        [Test]
        public void Cleanup_ServerNotRunning_ServerNotStopped()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            A.CallTo(() => serverListener.ServerActive).Returns(false);
            oidc.ServerListener = serverListener;

            oidc.Cleanup();

            A.CallTo(() => serverListener.StopServerImmediately()).MustNotHaveHappened();
        }

        [Test]
        public void Cleanup_LoggedOut_DoesNotRaiseLogoutEvent()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.ServerListener = A.Fake<IRedirectServerListener>();

            int eventCalls = 0;
            oidc.LogoutCompleted += delegate
            {
                eventCalls++;
            };

            oidc.Cleanup();

            Assert.AreEqual(0, eventCalls);
        }

        [Test]
        public void Cleanup_LoggedIn_RaisesLogoutEvent()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.ServerListener = A.Fake<IRedirectServerListener>();
            typeof(OpenIDConnectService).GetProperty("AccessToken").SetValue(oidc, "abcd");

            int eventCalls = 0;
            oidc.LogoutCompleted += delegate
            {
                eventCalls++;
            };

            oidc.Cleanup();

            Assert.AreEqual(1, eventCalls);
        }

        [UnityTest]
        public IEnumerator OpenLoginPageAsync_OpensLoginPageWithHttpRedirect()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            A.CallTo(() => server.GenerateListeningUri(A<string>.Ignored))
                .ReturnsLazily((string schema) => schema + "://127.0.0.1:1234");
            server.ListeningUri = "http://127.0.0.1:1234";
            oidc.ServerListener = server;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            A.CallTo(() => provider.OpenLoginPage(A<string[]>.Ignored, A<string>.That.IsEqualTo("http://127.0.0.1:1234")))
                .MustHaveHappened();
        }

        [UnityTest]
        public IEnumerator OpenLoginPageAsync_NoRedirectUriGiven_UsesDefaultPage()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = server;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            Assert.IsFalse(server.ResponseString.Contains("<meta http-equiv=\"Refresh\""));
        }

        [UnityTest]
        public IEnumerator OpenLoginpageAsync_RedirectUriGiven_RedirectUriInResponseString()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            oidc.RedirectURI = "http://test.com";
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = server;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            Debug.Log("Resp: " + server.ResponseString);
            Assert.IsTrue(server.ResponseString.Contains("<meta http-equiv=\"Refresh\" content=\"0; url = http://test.com\" />"));
        }

        [UnityTest]
        public IEnumerator OpenLoginPageAsync_Called_ServerStarted()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = server;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            A.CallTo(() => server.StartServer()).MustHaveHappened();
        }

        [UnityTest]
        public IEnumerator OpenLoginPageAsync_OidcProviderNull_LogsError()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.OidcProvider = null;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*OIDC provider is not set\w*"));

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);
        }

        [UnityTest]
        public IEnumerator OpenLoginPageAsync_ServerListenerNull_LogsError()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.OidcProvider = A.Fake<IOidcProvider>();
            oidc.ServerListener = null;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Redirect server listener is not set\w*"));

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);
        }

        [UnityTest]
        public IEnumerator OnRedirect_AuthFlow_ExtractsCode()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.AuthorizationFlow).Returns(AuthorizationFlow.AUTHORIZATION_CODE);
            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult("myAccessToken"));
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.GetAuthorizationCode(A<Dictionary<string, string>>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [UnityTest]
        public IEnumerator OnRedirect_AuthFlow_RetrievesAccessToken()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAuthorizationCode(A<Dictionary<string, string>>.Ignored)).Returns("myCode");
            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult("myAccessToken"));
            A.CallTo(() => oidcProvider.AuthorizationFlow).Returns(AuthorizationFlow.AUTHORIZATION_CODE);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync("myCode", A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.AreEqual("myAccessToken", oidc.AccessToken);
        }

        [UnityTest]
        public IEnumerator OnRedirect_ImplicitFlow_AccessTokenRetrieved()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorizationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            Task task = oidc.OpenLoginPageAsync();

            yield return AsyncTest.WaitForTask(task);

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            Assert.AreEqual("myAccessToken", oidc.AccessToken);
        }

        [UnityTest]
        public IEnumerator OnRedirect_RedirectContainsError_ErrorLogged()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            string errorMsg;
            A.CallTo(() => oidcProvider.ParametersContainError(A<Dictionary<string, string>>.Ignored, out errorMsg))
                .Returns(true)
                .AssignsOutAndRefParameters("This is a simulated fail");
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            oidc.ServerListener = serverListener;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated fail\w*"));

            Task task = oidc.OpenLoginPageAsync();
            yield return AsyncTest.WaitForTask(task);

            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.ParametersContainError(A<Dictionary<string, string>>.Ignored, out errorMsg))
                .MustHaveHappenedOnceExactly();
            Assert.IsTrue(string.IsNullOrEmpty(oidc.AccessToken));
        }

        [UnityTest]
        public IEnumerator OnRedirect_Success_EventRaised()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult("myAccessToken"));
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;
            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();

            int eventCalls = 0;

            oidc.LoginCompleted += delegate
            {
                eventCalls++;
            };
            Task task = oidc.OpenLoginPageAsync();
            yield return AsyncTest.WaitForTask(task);

            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);
            oidc.Update();

            Assert.AreEqual(1, eventCalls);
        }

        [Test]
        public void Logout_AccessTokenSet_AccessTokenCleared()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            typeof(OpenIDConnectService).GetProperty("AccessToken").SetValue(oidc, "abcd");

            oidc.Logout();
            Assert.IsEmpty(oidc.AccessToken);
        }

        [Test]
        public void Logout_EventSubscribed_EventRaised()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            int events = 0;
            oidc.LogoutCompleted += delegate
            {
                events++;
            };

            oidc.Logout();
            Assert.AreEqual(1, events);
        }

        [Test]
        public void IsLoggedIn_Default_ReturnsFalse()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            Assert.IsFalse(oidc.IsLoggedIn);
        }

        [UnityTest]
        public IEnumerator IsLoggedIn_SuccessfulLogin_ReturnsTrue()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorizationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            Task task = oidc.OpenLoginPageAsync();
            yield return AsyncTest.WaitForTask(task);

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            Assert.IsTrue(oidc.IsLoggedIn);
        }

        [UnityTest]
        public IEnumerator IsLoggedIn_SuccessfulLogout_ReturnsFalse()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorizationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            Task task = oidc.OpenLoginPageAsync();
            yield return AsyncTest.WaitForTask(task);

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Logout();

            Assert.IsFalse(oidc.IsLoggedIn);
        }

		[Test]
		public void LoginWithAccessToken_LoginCompletedEventRaised()
		{
			OpenIDConnectService oidc = new OpenIDConnectService();
			int events = 0;
			oidc.LoginCompleted += delegate
			{
				events++;
			};

			oidc.LoginWithAccessToken("myAccessToken");
			Assert.AreEqual(1, events);
		}

        [Test]
        public void LoginWithAccessToken_AccessTokenSet()
        {
			OpenIDConnectService oidc = new OpenIDConnectService();
            string accessToken = "myAccessToken";
			oidc.LoginWithAccessToken(accessToken);
			Assert.AreEqual(accessToken, oidc.AccessToken);
		}
	}
}
