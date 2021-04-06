using FakeItEasy;
using i5.Toolkit.Core.DeepLinkAPI;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.Utilities.UnityAdapters;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
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
        public void Initialize_AbsoluteURINonEmptyWithParams_WithParamsCalled()
        {
            linkingService.AddDeepLinkListener(dl);

            A.CallTo(() => appFake.AbsoluteURL).Returns("test://withParams?value=123");

            linkingService.Initialize(A.Fake<IServiceManager>());

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
            Assert.AreEqual(1, dl.TimesWithParamsCalled);
        }

        [Test]
        public void Initialize_AbsoluteURINonEmptyWithParams_ArgumentsSet()
        {
            linkingService.AddDeepLinkListener(dl);

            A.CallTo(() => appFake.AbsoluteURL).Returns("test://withParams?value=123");

            linkingService.Initialize(A.Fake<IServiceManager>());

            Assert.AreEqual("123", dl.DeepLinkArgs.Parameters["value"]);
        }

        [Test]
        public void Initialize_AbsoluteURINonEmptyNotRegistered_NoError()
        {
            A.CallTo(() => appFake.AbsoluteURL).Returns("test://notRegistered");
            
            linkingService.Initialize(A.Fake<IServiceManager>());
        }

        [Test]
        public void AddDeepLinkListener_NotContained_ListenerCountIncreased()
        {
            linkingService.AddDeepLinkListener(dl);
            Assert.AreEqual(1, linkingService.RegisteredListenersCount);
        }

        [Test]
        public void AddDeepLinkListener_AlreadyContained_DoesNotAddTwice()
        {
            linkingService.AddDeepLinkListener(dl);
            Assert.AreEqual(1, linkingService.RegisteredListenersCount);

            linkingService.AddDeepLinkListener(dl);
            Assert.AreEqual(1, linkingService.RegisteredListenersCount);
        }

        [Test]
        public void AddDeepLinkListener_TargetForAbsoluteURL_TriggersURL()
        {
            A.CallTo(() => appFake.AbsoluteURL).Returns("test://withoutParams");

            linkingService.AddDeepLinkListener(dl);

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void RemoveDeepLinkListener_Contained_CountDecreasedBy1()
        {
            linkingService.AddDeepLinkListener(dl);
            linkingService.RemoveDeepLinkListener(dl);
            Assert.AreEqual(0, linkingService.RegisteredListenersCount);
        }

        [Test]
        public void RemoveDeepLinkListener_NotContained_CountSame()
        {
            DeepLinkTestDefinition dl2 = new DeepLinkTestDefinition();
            linkingService.AddDeepLinkListener(dl2);
            linkingService.RemoveDeepLinkListener(dl);
            Assert.AreEqual(1, linkingService.RegisteredListenersCount);
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

            Assert.AreEqual("test", dl.DeepLinkArgs.Scheme);
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

        [Test]
        public void OnDeepLinkActivated_FaultyMethodTargeted_LogsError()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            LogAssert.Expect(UnityEngine.LogType.Error, new Regex(@"\w*Cannot deep-link\w*"));
            appFake.DeepLinkActivated += Raise.With("test://faulty");

            Assert.AreEqual(0, dl.TimesWithParamsCalled);
            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_FilteredMatching_CallsMethod()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("test://filtered");

            Assert.AreEqual(1, dl.TimesWithoutParamsCalled);
        }

        [Test]
        public void OnDeepLinkActivated_FilterNotMatching_CallsMethod()
        {
            linkingService.Initialize(A.Fake<IServiceManager>());
            linkingService.AddDeepLinkListener(dl);

            appFake.DeepLinkActivated += Raise.With("nonmatch://filtered");

            Assert.AreEqual(0, dl.TimesWithoutParamsCalled);
        }
    }
}
