using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.ModelImporters;
using i5.Toolkit.Core.ServiceCore;
using System.Threading.Tasks;
using i5.Toolkit.Core.TestHelpers;
using i5.Toolkit.Core.Caching;

namespace i5.Toolkit.Core.Tests.Caching
{
    public class FileCacheTests
    {
        /// <summary>
        /// Resets the scene to the standard test scene before executed each test
        /// </summary>
        [SetUp]
        public void ResetScene()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Check that the file cache is empty when being initialized with default values.
        /// </summary>
        [Test]
        public void FileCache_Initializes_With_Empty_Cache_Default()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);
            Assert.NotNull(fileCache);
            Assert.IsTrue(fileCache.filesInCache() == 0);
        }

        /// <summary>
        /// Check that loading a file from the web works.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AddOrUpdateInCache_Load_Web_File()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            Task<string> task = fileCache.addOrUpdateInCache("https://people.sc.fsu.edu/~jburkardt/data/obj/airboat.obj");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreNotEqual(res, "");
            FileAssert.Exists(res);
        }

        /// <summary>
        /// Checks that empty string is retuned when the given file does not exist.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AddOrUpdateInCache_Returns_Empty_String_When_File_Not_Exist()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            LogAssert.Expect(LogType.Error, "[i5.Toolkit.Core.Utilities.ContentLoaders.UnityWebRequestLoader] Get request to: notcorrectadress returned with error Cannot connect to destination host");
            Task<string> task = fileCache.addOrUpdateInCache("notcorrectadress");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.IsTrue(res == "");
            
        }

        /// <summary>
        /// Check that the isFileinCache function detects files that were loaded before
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator isFileInCache_True_when_loaded()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            Task<string> task = fileCache.addOrUpdateInCache("https://people.sc.fsu.edu/~jburkardt/data/obj/airboat.obj");

            yield return AsyncTest.WaitForTask(task);

            Assert.IsTrue(fileCache.isFileInCache("https://people.sc.fsu.edu/~jburkardt/data/obj/airboat.obj"));
        }

        /// <summary>
        /// Check that the isFileinCache function detects when a files was not loaded before
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator isFileInCache_Fasle_when_not_loaded()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            Task<string> task = fileCache.addOrUpdateInCache("https://people.sc.fsu.edu/~jburkardt/data/obj/airboat.obj");

            yield return AsyncTest.WaitForTask(task);

            Assert.IsFalse(fileCache.isFileInCache("other.obj"));
        }

        /// <summary>
        /// Check that the isFileinCache function detects when no files were ever loaded before
        /// </summary>
        /// <returns></returns>
        [Test]
        public void isFileInCache_Fasle_when_nothing_loaded()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            Assert.IsFalse(fileCache.isFileInCache("other.obj"));
        }
    }
}
