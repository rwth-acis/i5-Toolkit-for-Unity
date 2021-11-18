using FakeItEasy;
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Caching
{
    public class CacheAwareContentLoaderInitializationTests
    {
        private CacheAwareContentLoader loader;

        private const string testUrl = "https://test.org/myfile.txt";

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();

            loader = new CacheAwareContentLoader(null);
        }

        [Test]
        public void Constr_ContentLoaderInitializedWithUnityWebReq()
        {
            Assert.NotNull(loader.InternalContentLoader);
            Assert.IsTrue(loader.InternalContentLoader.GetType() == typeof(UnityWebRequestLoader));
        }

        [UnityTest]
        public IEnumerator LoadAsync_CacheUpdateFails_TakesOriginalUri()
        {
            loader.InternalContentLoader = A.Fake<IContentLoader<string>>();
            loader.Cache = A.Fake<IFileCache>();
            A.CallTo(() => loader.Cache.IsFileInCache(testUrl)).Returns(false);
            A.CallTo(() => loader.Cache.AddOrUpdateInCacheAsync(testUrl)).Returns(Task.FromResult(""));

            Task<WebResponse<string>> task = loader.LoadAsync(testUrl);

            yield return AsyncTest.WaitForTask(task);

            A.CallTo(() => loader.InternalContentLoader.LoadAsync(testUrl)).MustHaveHappenedOnceExactly();
        }

        [UnityTest]
        public IEnumerator LoadAsync_CacheNotAvailable_UsesInternalContentLoader()
        {
            loader.InternalContentLoader = A.Fake<IContentLoader<string>>();
            loader.Cache = null;

            Task<WebResponse<string>> task = loader.LoadAsync(testUrl);

            yield return AsyncTest.WaitForTask(task);

            A.CallTo(() => loader.InternalContentLoader.LoadAsync(testUrl)).MustHaveHappenedOnceExactly();
        }

    }
}
