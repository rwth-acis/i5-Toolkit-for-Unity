using FakeItEasy;
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.SystemAdapters;
using i5.Toolkit.Core.ServiceCore;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.Utilities;
using i5.Toolkit.Core.Utilities.ContentLoaders;
using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Caching
{
    public class FileCacheServicePersistenceTests
    {
        private FileCacheService fileCache;

        private string cacheLocation = Path.Combine(Application.temporaryCachePath, FileCacheService.persistentCacheFileName);

        private const string testCache = "{\"keys\":[\"https://test.org/myfile.obj\"," +
            "\"https://test.org/otherfile.txt\"]," +
            "\"values\":[{\"localFileName\":\"C:/Cache/myfile.obj\",\"fileHash\":\"hashResult\"," +
            "\"cacheDate\":\"17.11.2021 17:48:23\"}," +
            "{\"localFileName\":\"C:/Cache/otherfile.txt\",\"fileHash\":\"hashResult\"," +
            "\"cacheDate\":\"17.11.2021 17:48:23\"}]}";

        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();

            fileCache = new FileCacheService(sessionPersistence: true, directoryAccessor: A.Fake<IDirectoryAccessor>());
            fileCache.FileAccessor = A.Fake<IFileAccessor>();
            fileCache.ContentLoader = A.Fake<IContentLoader<string>>();
            fileCache.FileHasher = A.Fake<IFileHasher>();
            A.CallTo(() => fileCache.FileHasher.CalculateMD5Hash(A<string>.Ignored)).Returns("hashResult");
        }

        [Test]
        public void Initialize_CachePathExists_LoadsCache()
        {
            A.CallTo(() => fileCache.FileAccessor.Exists(A<string>.Ignored)).Returns(true);
            A.CallTo(() => fileCache.FileAccessor.ReadAllText(cacheLocation))
                .Returns(testCache);

            fileCache.Initialize(A.Fake<IServiceManager>());

            A.CallTo(() => fileCache.FileAccessor.ReadAllText(cacheLocation)).MustHaveHappened();

            Assert.AreEqual(2, fileCache.FileCount);
        }

        [Test]
        public void Initialize_FileMissing_EntryRemoved()
        {
            A.CallTo(() => fileCache.FileAccessor.Exists(cacheLocation)).Returns(true);
            // only one of the files is found
            A.CallTo(() => fileCache.FileAccessor.Exists("C:/Cache/myfile.obj")).Returns(true);
            A.CallTo(() => fileCache.FileAccessor.ReadAllText(cacheLocation))
                .Returns(testCache);

            fileCache.Initialize(A.Fake<IServiceManager>());

            Assert.AreEqual(1, fileCache.FileCount);
        }

        [Test]
        public void Initialize_CachePathMissing_Log()
        {
            LogAssert.Expect(LogType.Log, new Regex(@"\w*No previous cache session detected\w*"));

            A.CallTo(() => fileCache.FileAccessor.Exists(cacheLocation)).Returns(false);

            fileCache.Initialize(A.Fake<IServiceManager>());
        }

        [UnityTest]
        public IEnumerator Cleanup_WritesCache()
        {
            A.CallTo(() => fileCache.ContentLoader.LoadAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>("content", null, 200)));

            string contentWrittenToFile = "";

            A.CallTo(() => fileCache.FileAccessor.WriteAllText(A<string>.Ignored, A<string>.Ignored)).Invokes((x) => contentWrittenToFile = x.Arguments[1].ToString());

            fileCache.Initialize(A.Fake<IServiceManager>());
            Task<string> task = fileCache.AddOrUpdateInCache("https://test.org/myfile.obj");

            yield return AsyncTest.WaitForTask(task);

            task = fileCache.AddOrUpdateInCache("https://test.org/otherfile.txt");

            yield return AsyncTest.WaitForTask(task);

            fileCache.Cleanup();

            Assert.IsTrue(contentWrittenToFile.Contains("https://test.org/myfile.obj"));
            Assert.IsTrue(contentWrittenToFile.Contains("https://test.org/otherfile.txt"));
        }

        [Test]
        public void Cleanup_WritingThrowsError_ErrorLogged()
        {
            LogAssert.Expect(LogType.Error, new Regex(@"\w*not able to store\w*"));

            A.CallTo(() => fileCache.FileAccessor.WriteAllText(A<string>.Ignored, A<string>.Ignored)).Throws<IOException>();

            fileCache.Initialize(A.Fake<IServiceManager>());

            fileCache.Cleanup();
        }
    }
}
