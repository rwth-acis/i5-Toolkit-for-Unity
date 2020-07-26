using FakeItEasy;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.TestUtilities;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.OpenIDConnectClient
{
    public class LearningLayersOIDCProviderTests
    {
        [Test]
        public void Constructor_Initialized_ContentLoaderNotNull()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();

            Assert.IsNotNull(lloidc.RestConnector);
        }

        [Test]
        public void Constructor_Initialized_JsonWrapperNotNull()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();

            Assert.IsNotNull(lloidc.JsonSerializer);
        }

        [UnityTest]
        public IEnumerator GetAccessCodeFromTokenAsync_NoClientData_ReturnsEmptyString()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            lloidc.RestConnector = A.Fake<IRestConnector>();

            LogAssert.Expect(LogType.Error, 
                new Regex(@"\w*No client data supplied for the OpenID Connect Client\w*"));

            Task<string> task = lloidc.GetAccessTokenFromCodeAsync("", "");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreEqual("", res);
        }

        [UnityTest]
        public IEnumerator GetAccessCodeFromTokenAsync_WebResponseSuccess_ReturnsToken()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.PostAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>("json string", null, 200)));
            lloidc.RestConnector = restConnector;
            lloidc.ClientData = A.Fake<ClientData>();
            LearningLayersAuthorizationFlowAnswer answer = new LearningLayersAuthorizationFlowAnswer();
            answer.access_token = "myAccessToken";
            IJsonSerializer serializer = A.Fake<IJsonSerializer>();
            A.CallTo(() => serializer.FromJson<LearningLayersAuthorizationFlowAnswer>(A<string>.Ignored))
                .Returns(answer);
            lloidc.JsonSerializer = serializer;

            Task<string> task = lloidc.GetAccessTokenFromCodeAsync("", "");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreEqual(answer.access_token, res);
        }

        [UnityTest]
        public IEnumerator GetAccessCodeFromTokenAsync_WebResponseSuccess_ReturnsEmptyToken()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.PostAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>("my error", 400)));
            lloidc.RestConnector = restConnector;
            lloidc.ClientData = A.Fake<ClientData>();
            lloidc.JsonSerializer = A.Fake<IJsonSerializer>();

            Task<string> task = lloidc.GetAccessTokenFromCodeAsync("", "");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreEqual("", res);
        }

        [Test]
        public void GetAccessToken_TokenProvided_ExtractsToken()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();
            redirectParameters.Add("token", "myAccessToken");

            string res = lloidc.GetAccessToken(redirectParameters);

            Assert.AreEqual("myAccessToken", res);
        }

        [Test]
        public void GetAccessToken_TokenNotProvided_ReturnsEmptyToken()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Redirect parameters did not contain access token\w*"));

            string res = lloidc.GetAccessToken(redirectParameters);

            Assert.AreEqual("", res);
        }

        //[UnityTest]
        //public IEnumerator GetUserInfoAsync_WebResponseSuccessful_ReturnsUserInfo()
        //{
        //    LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
        //    IRestConnector restConnector = A.Fake<IRestConnector>();
        //    A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
        //        .Returns(new WebResponse<string>("answer", null, 200));
        //    lloidc.RestConnector = restConnector;
        //    LearningLayersUserInfo userInfo = new LearningLayersUserInfo("tester", "tester@test.com", "Tester");
        //    IJsonSerializer serializer = A.Fake<IJsonSerializer>();
        //    A.CallTo(() => serializer.FromJson<object>(A<string>.Ignored))
        //        .Returns(userInfo);
        //    lloidc.JsonSerializer = serializer;

        //    Task<IUserInfo> task = lloidc.GetUserInfoAsync("");

        //    yield return AsyncTest.WaitForTask(task);

        //    IUserInfo res = task.Result;

        //    Assert.AreEqual(userInfo.Email, res.Email);
        //}
    }
}
