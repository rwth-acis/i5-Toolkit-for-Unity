using System.Collections;
using System.Collections.Generic;
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
        public void ClientDataLoader_Initialized_DefaultNotNull()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            Assert.NotNull(oidc.ClientDataLoader);
        }

        [Test]
        public void Initialize_Initialized_LoadsClientData()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IClientDataLoader dataLoader = A.Fake<IClientDataLoader>();
            A.CallTo(() => dataLoader.LoadClientDataAsync()).Returns(Task.FromResult(A.Fake<ClientData>()));
            BaseServiceManager serviceManager = A.Fake<BaseServiceManager>();
            oidc.ClientDataLoader = dataLoader;

            oidc.Initialize(serviceManager);

            A.CallTo(() => oidc.ClientDataLoader.LoadClientDataAsync()).MustHaveHappened(Repeated.Once);
        }

        [Test]
        public void OpenLoginPage_Called_OpensLoginPage()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            IOidcProvider provider = A.Fake<IOidcProvider>();
            oidc.OidcProvider = provider;
            IRedirectServerListener server = A.Fake<IRedirectServerListener>();
            oidc.ServerListener = server;

            oidc.OpenLoginPage();
            A.CallTo(() => provider.OpenLoginPage(A<string[]>.Ignored, A<string>.Ignored)).MustHaveHappened();
        }

        [Test]
        public void Logout_AccessTokenSet_AccessTokenCleared()
        {
            OpenIDConnectService oidc = new OpenIDConnectService();
            typeof(OpenIDConnectService).GetProperty("AccessToken").SetValue(oidc, "abcd");
            Assert.IsNotEmpty(oidc.AccessToken);

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
    }
}
