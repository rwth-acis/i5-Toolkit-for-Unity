using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FakeItEasy;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestUtilities;
using NUnit.Framework;
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
        public void ClientDataLoader_DefaultNotNull()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            Assert.NotNull(oidc.ClientDataLoader);
        }

        [Test]
        public void ServerListener_DefaultNotNull()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            Assert.NotNull(oidc.ServerListener);
        }

        [Test]
        public void Initialize_NoClientDataGiven_LoadsClientData()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IClientDataLoader dataLoader = A.Fake<IClientDataLoader>();
            A.CallTo(() => dataLoader.LoadClientDataAsync()).Returns(Task.FromResult(A.Fake<ClientData>()));
            BaseServiceManager serviceManager = A.Fake<BaseServiceManager>();
            oidc.ClientDataLoader = dataLoader;

            oidc.Initialize(serviceManager);

            A.CallTo(() => oidc.ClientDataLoader.LoadClientDataAsync()).MustHaveHappenedOnceExactly();
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

        [Test]
        public void OpenLoginPage_SchemaNotChanged_OpensLoginPageWithHttpRedirect()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            A.CallTo(() => server.GenerateRedirectUri(A<string>.Ignored))
                .ReturnsLazily((string schema) => schema + "://127.0.0.1:1234");
            oidc.ServerListener = server;

            oidc.OpenLoginPage();
            A.CallTo(() => provider.OpenLoginPage(A<string[]>.Ignored, A<string>.That.StartsWith("http://")))
                .MustHaveHappened();
        }

        [Test]
        public void OpenLoginPage_SchemaChanged_OpensLoginPageWithSchemaRedirect()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            A.CallTo(() => server.GenerateRedirectUri(A<string>.Ignored))
                .ReturnsLazily((string schema) => schema + "://127.0.0.1:1234");
            oidc.ServerListener = server;
            oidc.UriSchema = "test";

            oidc.OpenLoginPage();
            A.CallTo(() => provider.OpenLoginPage(A<string[]>.Ignored, A<string>.That.StartsWith("test://")))
                .MustHaveHappened();
        }

        [Test]
        public void OpenLoginPage_Called_ServerStarted()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = server;

            oidc.OpenLoginPage();
            A.CallTo(() => server.StartServer()).MustHaveHappened();
        }

        [Test]
        public void OpenLoginPage_OidcProviderNull_LogsError()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.OidcProvider = null;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*OIDC provider is not set\w*"));

            oidc.OpenLoginPage();
        }

        [Test]
        public void OpenLoginPage_ServerListenerNull_LogsError()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            oidc.OidcProvider = A.Fake<IOidcProvider>();
            oidc.ServerListener = null;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Redirect server listener is not set\w*"));

            oidc.OpenLoginPage();
        }

        [Test]
        public void OnRedirect_AuthFlow_ExtractsCode()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.AuthorzationFlow).Returns(AuthorizationFlow.AUTHORIZATION_CODE);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            oidc.OpenLoginPage();

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.GetAuthorizationCode(A<Dictionary<string, string>>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void OnRedirect_AuthFlow_RetrievesAccessToken()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAuthorizationCode(A<Dictionary<string, string>>.Ignored)).Returns("myCode");
            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult("myAccessToken"));
            A.CallTo(() => oidcProvider.AuthorzationFlow).Returns(AuthorizationFlow.AUTHORIZATION_CODE);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            oidc.OpenLoginPage();

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.GetAccessTokenFromCodeAsync("myCode", A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.AreEqual("myAccessToken", oidc.AccessToken);
        }

        [Test]
        public void OnRedirect_ImplicitFlow_AccessTokenRetrieved()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorzationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            oidc.OpenLoginPage();

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            Assert.AreEqual("myAccessToken", oidc.AccessToken);
        }

        [Test]
        public void OnRedirect_RedirectContainsError_ErrorLogged()
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

            oidc.OpenLoginPage();

            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            A.CallTo(() => oidcProvider.ParametersContainError(A<Dictionary<string, string>>.Ignored, out errorMsg))
                .MustHaveHappenedOnceExactly();
            Assert.IsTrue(string.IsNullOrEmpty(oidc.AccessToken));
        }

        [Test]
        public void OnRedirect_Success_EventRaised()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;
            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();

            int eventCalls = 0;

            oidc.LoginCompleted += delegate
            {
                eventCalls++;
            };
            oidc.OpenLoginPage();

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

        [Test]
        public void IsLoggedIn_SuccessfulLogin_ReturnsTrue()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorzationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            oidc.OpenLoginPage();

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Update();

            Assert.IsTrue(oidc.IsLoggedIn);
        }

        [Test]
        public void IsLoggedIn_SuccessfulLogout_ReturnsFalse()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider oidcProvider = A.Fake<IOidcProvider>();
            A.CallTo(() => oidcProvider.GetAccessToken(A<Dictionary<string, string>>.Ignored)).Returns("myAccessToken");
            A.CallTo(() => oidcProvider.AuthorzationFlow).Returns(AuthorizationFlow.IMPLICIT);
            oidc.OidcProvider = oidcProvider;
            IRedirectServerListener serverListener = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = serverListener;

            oidc.OpenLoginPage();

            RedirectReceivedEventArgs redirectReceivedEventArgs = A.Fake<RedirectReceivedEventArgs>();
            serverListener.RedirectReceived += Raise.With(redirectReceivedEventArgs);

            oidc.Logout();

            Assert.IsFalse(oidc.IsLoggedIn);
        }
    }
}
