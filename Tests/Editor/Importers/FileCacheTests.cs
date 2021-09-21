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
        /// 
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

        [UnityTest]
        public IEnumerator AddOrUpdateInCache_Loads_File()
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

        [UnityTest]
        public IEnumerator AddOrUpdateInCache_Returns_Empty_String_When_File_Not_Exist()
        {
            FileCache fileCache = new FileCache();
            IServiceManager serviceManager = A.Fake<IServiceManager>();
            fileCache.Initialize(serviceManager);

            Task<string> task = fileCache.addOrUpdateInCache("notcorrectadress");

            yield return AsyncTest.WaitForTask(task);

            string res = task.Result;

            Assert.AreEqual("", res);
        }
    }
}
