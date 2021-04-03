using FakeItEasy;
using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using System;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.DeepLinkAPI
{
    public class DeepLinkServiceTests
    {
        private DeepLinkingService linkingService;
        private IApplication appFake;
        private DeepLinkTestDefinition dl;

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
            linkingService = new DeepLinkingService();
            appFake = A.Fake<IApplication>();
            dl = new DeepLinkTestDefinition();

            linkingService.ApplicationAPI = appFake;
        }

        [Test]
        public void Initialize_SubscribesToDeepLinkEvent()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            A.CallTo(appFake).Where(x => x.Method.Name.Equals("add_DeepLinkActivated")).MustHaveHappened();
        }

        [Test]
        public void Cleanup_UnsubscribesFromDeepLinkEvent()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.Cleanup();
            A.CallTo(appFake).Where(x => x.Method.Name.Equals("remove_DeepLinkActivated")).MustHaveHappened();
        }

        [Test]
        public void Initialize_AbsoluteURINonEmpty_CallsTarget()
        {
            linkingService.AddDeepLinkListener(dl);

            A.CallTo(() => appFake.AbsoluteURL).Returns("test://withoutParams");

            linkingService.Initialize(A.Fake<IServiceManager>());

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void Initialize_AbsoluteURINonEmptyNotRegistered_NoError()
        {
            A.CallTo(() => appFake.AbsoluteURL).Returns("test://notRegistered");
            
            linkingService.Initialize(A.Fake<IServiceManager>());
        }

        [Test]
        public void OnDeepLinkActivated_PathRegistered_CallsPath()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withoutParams");

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_RecieverNotRegistered_NoAction()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());

            appFake.DeepLinkActivated += Raise.With("test://withoutParams");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_UnknownPath_NoAction()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://unknown");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_Param_MethodCalled()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withParams?value=123");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
            Assert.AreEqual(1, dl.TimesWithParamsCalled);
        }

        [Test]
        public void OnDeepLinkActiavted_Param_ParametersCorrect()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withParams?value=123&value2=test");

            Assert.AreEqual("123", dl.DeepLinkArgs.Parameters["value"]);
            Assert.AreEqual("test", dl.DeepLinkArgs.Parameters["value2"]);
        }

        [Test]
        public void OnDeepLinkActivated_Param_DeepLinkCorrect()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            string deepLink = "test://withParams/?value=123";

            appFake.DeepLinkActivated += Raise.With(deepLink);

            Assert.AreEqual(deepLink.ToLower(), dl.DeepLinkArgs.DeepLink.ToString());
        }

        [Test]
        public void OnDeepLinkActivated_Param_ProtocolCorrect()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            string deepLink = "test://withParams/?value=123";

            appFake.DeepLinkActivated += Raise.With(deepLink);

            Assert.AreEqual("test", dl.DeepLinkArgs.Protocol);
        }

        [Test]
        public void OnDeepLinkActivated_ParamInURLNotInMethod_MethodCalled()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withoutParams?value=123");

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
            Assert.AreEqual(0, dl.TimesWithParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_ParamInMethodNotInURL_MethodCalled()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withParams");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
            Assert.AreEqual(1, dl.TimesWithParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_ParamInMethodNotInURL_ParametersEmpty()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://withParams");

            Assert.AreEqual(0, dl.DeepLinkArgs.Parameters.Count);
        }

        [Test]
        public void OnDeepLinkActivated_ParamInMethodNotInURL_DeepLinkCorrect()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            string url = "test://withParams/";

            appFake.DeepLinkActivated += Raise.With(url);

            Assert.AreEqual(url.ToLower(), dl.DeepLinkArgs.DeepLink.ToString());
        }

        [Test]
        public void OnDeepLinkActivated_MultipleTargets_AllCalled()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://duplicate");

            Assert.AreEqual(2, dl.TimesWithoutParamsCalled);
            Assert.AreEqual(1, dl.TimesWithParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_MultiplePaths_CorrectPath()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            string url = "test://multiPaths2/";

            appFake.DeepLinkActivated += Raise.With(url);

            Assert.AreEqual(url.ToLower(), dl.DeepLinkArgs.DeepLink.ToString());

            url = "test://multiPaths/";
            appFake.DeepLinkActivated += Raise.With(url);

            Assert.AreEqual(url.ToLower(), dl.DeepLinkArgs.DeepLink.ToString());
        }
    }
}
