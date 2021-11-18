using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Caching
{
    public class CacheAwareContentLoaderTests
    {
        private CacheAwareContentLoader loader;

        private const string testUrl = "https://test.org/myfile.txt";

        private const string addedToCachePseudoPath = "addedToCache";
        private const string retrievedFromCachePseudoPath = "retrievedFromCache";

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();

            loader = new CacheAwareContentLoader(A.Fake<IFileCache>());
            loader.InternalContentLoader = A.Fake<IContentLoader<string>>();

            // fake cache returns different "addresses" depending on the way how content was retrieved
            A.CallTo(() => loader.Cache.AddOrUpdateInCacheAsync(testUrl))
                .Returns(Task.FromResult(addedToCachePseudoPath));
            A.CallTo(() => loader.Cache.GetCachedFileLocation(testUrl)).Returns(retrievedFromCachePseudoPath);
            // return input path as file content so that we can see how the content was loaded
            A.CallTo(() => loader.InternalContentLoader.LoadAsync(A<string>.Ignored))
                .ReturnsLazily((string path) => Task.FromResult(new WebResponse<string>(path, null, 200)));
        }

        [UnityTest]
        public IEnumerator LoadAsync_FileInCache_TakesCachedFile()
        {
            A.CallTo(() => loader.Cache.IsFileInCache(testUrl)).Returns(true);

            Task<WebResponse<string>> task = loader.LoadAsync(testUrl);

            yield return AsyncTest.WaitForTask(task);

            Assert.AreEqual(retrievedFromCachePseudoPath, task.Result.Content);
            A.CallTo(() => loader.Cache.AddOrUpdateInCacheAsync(testUrl)).MustNotHaveHappened();
        }

        [UnityTest]
        public IEnumerator LoadAsync_FileNotInCache_CachesFile()
        {
            A.CallTo(() => loader.Cache.IsFileInCache(testUrl)).Returns(false);

            Task<WebResponse<string>> task = loader.LoadAsync(testUrl);

            yield return AsyncTest.WaitForTask(task);

            A.CallTo(() => loader.Cache.AddOrUpdateInCacheAsync(testUrl)).MustHaveHappenedOnceExactly();
        }

        [UnityTest]
        public IEnumerator LoadAsync_FileNotInCache_TakesNewCacheContent()
        {
            A.CallTo(() => loader.Cache.IsFileInCache(testUrl)).Returns(false);

            Task<WebResponse<string>> task = loader.LoadAsync(testUrl);

            yield return AsyncTest.WaitForTask(task);

            Assert.AreEqual(addedToCachePseudoPath, task.Result.Content);
        }
    }
}
