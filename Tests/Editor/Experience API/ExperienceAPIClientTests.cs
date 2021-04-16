using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ExperienceAPI;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.ExperienceAPI
{
    public class ExperienceAPIClientTests
    {
        private const string apiTarget = "http://test.org/xAPI";
        private const string authToken = "Auth test";
        private const string version = "1.2.3";

        private ExperienceAPIClient client;
        private IRestConnector webConnector;

        [SetUp]
        public void Setup()
        {
            EditModeTestUtilities.ResetScene();
            webConnector = A.Fake<IRestConnector>();
            client = new ExperienceAPIClient(new Uri(apiTarget), authToken, "1.2.3");
            client.WebConnector = webConnector;
        }

        [Test]
        public void Constr_XApiEndpointSetByParam()
        {
            Assert.AreEqual(new Uri(apiTarget), client.XApiEndpoint);
        }

        [Test]
        public void Constr_AuthorizationTokenSetByParam()
        {
            Assert.AreEqual(authToken, client.AuthorizationToken);
        }

        [Test]
        public void Constr_VersionSetByParam()
        {
            Assert.AreEqual(version, client.Version);
        }

        [Test]
        public void SendStatement_PrimitiveParamsOnlyMail_ActorMailCorrect()
        {
            string mailAddress = "tester@i5toolkit.com";

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual($"mailto:{mailAddress}", res.actor.mbox);
                });

            Task<WebResponse<string>> task = client.SendStatement(mailAddress, "", "");

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_PrimitiveParamsWithMailto_ActorMailCorrect()
        {
            string mailAddress = "mailto:tester@i5toolkit.com";

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(mailAddress, res.actor.mbox);
                });

            Task<WebResponse<string>> task = client.SendStatement(mailAddress, "", "");

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_PrimitiveParams_VerbIdCorrect()
        {
            string verbId = "http://test.org/myVerbId";

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(verbId, res.verb.id);
                });

            Task<WebResponse<string>> task = client.SendStatement("", verbId, "");

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_PrimitiveParams_ObjectIdCorrect()
        {
            string objectId = "http://test.org/myObjectId";

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(objectId, res.@object.id);
                });

            Task<WebResponse<string>> task = client.SendStatement("", "", objectId);

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_ObjectParams_ActorCorrect()
        {
            Actor actor = new Actor()
            {
                mbox = "mailto:tester@i5toolkit.com"
            };

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(actor, res.actor);
                });

            Task<WebResponse<string>> task = client.SendStatement(actor, new Verb(), new XApiObject());

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_ObjectParams_VerbCorrect()
        {
            Verb verb = new Verb()
            {
                id = "http://test.org/myVerbId"
            };

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(verb, res.verb);
                });

            Task<WebResponse<string>> task = client.SendStatement(new Actor(), verb, new XApiObject());

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_ObjectParams_ObjectCorrect()
        {
            XApiObject obj = new XApiObject()
            {
                id = "http://test.org/myObjectId"
            };

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(obj, res.@object);
                });

            Task<WebResponse<string>> task = client.SendStatement(new Actor(), new Verb(), obj);

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SendStatement_StatementParam_Correct()
        {
            Statement statement = new Statement
                ("tester@i5toolkit.com",
                "http://test.org/myVerbId",
                "http://test.org/myObjectId");

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .Invokes(
                (string uri, byte[] bytes, Dictionary<string, string> headers) =>
                {
                    Debug.Log("Checked web call");
                    string json = Encoding.UTF8.GetString(bytes);
                    Statement res = JsonUtility.FromJson<Statement>(json);
                    Assert.AreEqual(statement, res);
                });

            Task<WebResponse<string>> task = client.SendStatement(statement);

            AsyncTest.WaitForTask(task);

            A.CallTo(() => webConnector.PostAsync(A<string>.Ignored,
                A<byte[]>.Ignored,
                A<Dictionary<string, string>>.Ignored))
                .MustHaveHappenedOnceExactly();
        }
    }
}
