using FakeItEasy;
using i5.Toolkit.Core.OpenIDConnectClient;
using i5.Toolkit.Core.TestHelpers;
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

            Assert.IsEmpty(res);
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
        public IEnumerator GetAccessCodeFromTokenAsync_WebResponseFailed_ReturnsEmptyToken()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.PostAsync(A<string>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>("my error", 400)));
            lloidc.RestConnector = restConnector;
            lloidc.ClientData = A.Fake<ClientData>();
            lloidc.JsonSerializer = A.Fake<IJsonSerializer>();

            LogAssert.Expect(LogType.Error,
                new Regex(@"\w*my error\w*"));

            Task<string> task = lloidc.GetAccessTokenFromCodeAsync("", "");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.IsEmpty(res);
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

            Assert.IsEmpty(res);
        }

        [UnityTest]
        public IEnumerator GetUserInfoAsync_WebResponseSuccessful_ReturnsUserInfo()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
                .Returns(new WebResponse<string>("answer", null, 200));
            lloidc.RestConnector = restConnector;
            LearningLayersUserInfo userInfo = new LearningLayersUserInfo("tester", "tester@test.com", "Tester");
            IJsonSerializer serializer = A.Fake<IJsonSerializer>();
            A.CallTo(() => serializer.FromJson<LearningLayersUserInfo>(A<string>.Ignored))
                .Returns(userInfo);
            lloidc.JsonSerializer = serializer;

            Task<IUserInfo> task = lloidc.GetUserInfoAsync("");

            yield return AsyncTest.WaitForTask(task);

            IUserInfo res = task.Result;

            Assert.AreEqual(userInfo.Email, res.Email);
        }

        [UnityTest]
        public IEnumerator GetUserInfoAsync_WebResponseFailed_ReturnsNull()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
                .Returns(new WebResponse<string>("This is a simulated error", 400));
            lloidc.RestConnector = restConnector;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated error\w*"));

            Task<IUserInfo> task = lloidc.GetUserInfoAsync("");

            yield return AsyncTest.WaitForTask(task);

            IUserInfo res = task.Result;

            Assert.IsNull(res);
        }

        [UnityTest]
        public IEnumerator GetUserInfoAsync_JsonParseFailed_ReturnsNull()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
                .Returns(new WebResponse<string>("answer", null, 200));
            lloidc.RestConnector = restConnector;
            LearningLayersUserInfo userInfo = new LearningLayersUserInfo("tester", "tester@test.com", "Tester");
            IJsonSerializer serializer = A.Fake<IJsonSerializer>();
            A.CallTo(() => serializer.FromJson<LearningLayersUserInfo>(A<string>.Ignored))
                .Returns(null);
            lloidc.JsonSerializer = serializer;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Could not parse user info\w*"));

            Task<IUserInfo> task = lloidc.GetUserInfoAsync("");

            yield return AsyncTest.WaitForTask(task);

            IUserInfo res = task.Result;

            Assert.IsNull(res);
        }

        [UnityTest]
        public IEnumerator CheckAccessTokenAsync_WebResponseSuccessful_ReturnsTrue()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
                .Returns(new WebResponse<string>("answer", null, 200));
            lloidc.RestConnector = restConnector;
            LearningLayersUserInfo userInfo = new LearningLayersUserInfo("tester", "tester@test.com", "Tester");
            IJsonSerializer serializer = A.Fake<IJsonSerializer>();
            A.CallTo(() => serializer.FromJson<LearningLayersUserInfo>(A<string>.Ignored))
                .Returns(userInfo);
            lloidc.JsonSerializer = serializer;

            Task<bool> task = lloidc.CheckAccessTokenAsync("");

            yield return AsyncTest.WaitForTask(task);

            bool res = task.Result;

            Assert.IsTrue(res);
        }

        [UnityTest]
        public IEnumerator CheckAccessTokenAsync_WebResponseFailed_ReturnsFalse()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IRestConnector restConnector = A.Fake<IRestConnector>();
            A.CallTo(() => restConnector.GetAsync(A<string>.Ignored))
                .Returns(new WebResponse<string>("This is a simulated error", 400));
            lloidc.RestConnector = restConnector;

            LogAssert.Expect(LogType.Error, new Regex(@"\w*This is a simulated error\w*"));

            Task<bool> task = lloidc.CheckAccessTokenAsync("");

            yield return AsyncTest.WaitForTask(task);

            bool res = task.Result;

            Assert.IsFalse(res);
        }

        [Test]
        public void OpenLoginPage_UriGiven_BrowserOpened()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            IBrowser browser = A.Fake<IBrowser>();
            lloidc.Browser = browser;
            lloidc.ClientData = A.Fake<ClientData>();
            string[] testScopes = new string[] { "testScope" };

            lloidc.OpenLoginPage(testScopes, "http://www.test.com");

            A.CallTo(() => browser.OpenURL(A<string>.That.Contains("http://www.test.com"))).MustHaveHappenedOnceExactly();
            A.CallTo(() => browser.OpenURL(A<string>.That.Contains("testScope"))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAuthorizationCode_CodeProvided_ExtractsCode()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();
            redirectParameters.Add("code", "myCode");
            string res = lloidc.GetAuthorizationCode(redirectParameters);

            Assert.AreEqual("myCode", res);
        }

        [Test]
        public void GetAuthorizationCode_CodeNotProvided_ReturnsEmptyString()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();

            LogAssert.Expect(LogType.Error, new Regex(@"\w*Redirect parameters did not contain authorization code\w*"));

            string res = lloidc.GetAuthorizationCode(redirectParameters);

            Assert.IsEmpty(res);
        }

        [Test]
        public void ParametersContainError_NoError_ReturnsFalse()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();

            bool res = lloidc.ParametersContainError(redirectParameters, out string message);

            Assert.IsFalse(res);
        }

        [Test]
        public void ParametersContainError_NoError_ErrorMessageEmpty()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();

            bool res = lloidc.ParametersContainError(redirectParameters, out string message);

            Assert.IsEmpty(message);
        }

        [Test]
        public void ParametersContainError_ErrorGiven_ReturnsTrue()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();
            redirectParameters.Add("error", "This is a simulated error");

            bool res = lloidc.ParametersContainError(redirectParameters, out string message);

            Assert.IsTrue(res);
        }

        [Test]
        public void ParametersContainError_ErrorGiven_ErrorMessageSet()
        {
            LearningLayersOIDCProvider lloidc = new LearningLayersOIDCProvider();
            Dictionary<string, string> redirectParameters = new Dictionary<string, string>();
            string errorMsg = "This is a simulated error";
            redirectParameters.Add("error", errorMsg);

            bool res = lloidc.ParametersContainError(redirectParameters, out string message);

            Assert.AreEqual(errorMsg, message);
        }
    }
}
