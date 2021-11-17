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
    public class FileCacheServiceTests
    {
        private FileCacheService fileCache;

        private const string expectedContent = "content";
        private const string url = "https://test.org/myfile.obj";

        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();

            fileCache = new FileCacheService();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            fileCache.FileAccessor = A.Fake<IFileAccessor>();
            fileCache.DirectoryAccessor = A.Fake<IDirectoryAccessor>();
            fileCache.ContentLoader = A.Fake<IContentLoader<string>>();
            A.CallTo(() => fileCache.ContentLoader.LoadAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>(expectedContent, null, 200)));
            fileCache.FileHasher = A.Fake<IFileHasher>();
            A.CallTo(() => fileCache.FileHasher.CalculateMD5Hash(A<string>.Ignored)).Returns("myhash");
        }

        /// <summary>
        /// Checks that the file cache is empty when being initialized with default values.
        /// </summary>
        [Test]
        public void Initialize_WithEmptyCache_DefaultValue()
        {
            Assert.NotNull(fileCache);
            Assert.IsTrue(fileCache.FileCount == 0);
        }

        /// <summary>
        /// Checks that the correct path is returned.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AddOrUpdateInCache_LoadWebFile_ReturnsPath()
        {
            string url = "https://test.org/myfile.obj";

            Task<string> task = fileCache.AddOrUpdateInCache(url);

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreEqual(Path.Combine(Application.temporaryCachePath, "myfile.obj"), res);
        }

        /// <summary>
        /// Checks that the file is written to storage
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AddOrUpdateInCache_LoadWebFile_WritesFile()
        {
            Task<string> task = fileCache.AddOrUpdateInCache(url);

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            A.CallTo(() => fileCache.FileAccessor.WriteAllText(res, expectedContent)).MustHaveHappenedOnceExactly();
        }

        /// <summary>
        /// Checks that empty string is retuned when the given file cannot be retrieved
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AddOrUpdateInCache_WebError_ReturnsEmptyPath()
        {
            LogAssert.Expect(LogType.Error, new Regex(@"\w*Could not retrieve file\w*"));

            A.CallTo(() => fileCache.ContentLoader.LoadAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new WebResponse<string>("myerror", 400)));

            Task<string> task = fileCache.AddOrUpdateInCache(url);

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.IsTrue(string.IsNullOrEmpty(res));
        }

        /// <summary>
        /// Check that the IsFileinCache function detects files that were loaded before
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator IsFileInCache_FileExists_ReturnsTrue()
        {
            Task<string> task = fileCache.AddOrUpdateInCache(url);

            yield return AsyncTest.WaitForTask(task);

            string path = task.Result;

            // fake that the file is saved
            A.CallTo(() => fileCache.FileAccessor.Exists(path)).Returns(true);

            Assert.IsTrue(fileCache.IsFileInCache(url));
        }

        /// <summary>
        /// Check that the IsFileinCache function detects when a files was not loaded before
        /// </summary>
        /// <returns></returns>
        [Test]
        public void IsFileInCache_FileDoesNotExist_ReturnsFalse()
        {
            Assert.IsFalse(fileCache.IsFileInCache(url));
            Assert.IsFalse(fileCache.IsFileInCache("other.obj"));
        }
    }
}
