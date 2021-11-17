using FakeItEasy;
using i5.Toolkit.Core.Caching;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Experimental.SystemAdapters;
using NUnit.Framework;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.Caching
{
    public class FileCacheServiceConstrTests
    {
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void Constr_CacheLocationOverrideExists_IsSet()
        {
            string overrideLocation = @"C:\MyDirectory\OverrideCache";

            IDirectoryAccessor dirAccessor = A.Fake<IDirectoryAccessor>();
            A.CallTo(() => dirAccessor.Exists(overrideLocation)).Returns(true);

            FileCacheService fileCache = new FileCacheService(directoryAccessor: dirAccessor, cacheLocationOverride: overrideLocation);

            Assert.AreEqual(overrideLocation, fileCache.CacheLocation);
        }

        [Test]
        public void Constr_CacheLocationOverrideDoesNotExist_StaysDefault()
        {
            LogAssert.Expect(LogType.Error, new Regex(@"\w*Could not override cache location\w*"));

            string overrideLocation = @"C:\MyDirectory\OverrideCache";

            IDirectoryAccessor dirAccessor = A.Fake<IDirectoryAccessor>();
            A.CallTo(() => dirAccessor.Exists(overrideLocation)).Returns(false);

            FileCacheService fileCache = new FileCacheService(directoryAccessor: dirAccessor, cacheLocationOverride: overrideLocation);

            Assert.AreEqual(Application.temporaryCachePath, fileCache.CacheLocation);
        }

        [Test]
        public void Constr_NoCacheOverride_DefaultTempCacheSet()
        {
            FileCacheService fileCache = new FileCacheService(cacheLocationOverride: null);

            Assert.AreEqual(Application.temporaryCachePath, fileCache.CacheLocation);
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Constr_SessionPersistence_Set(bool value)
        {
            FileCacheService fileCache = new FileCacheService(sessionPersistence: value);

            Assert.AreEqual(value, fileCache.SessionPersistence);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void Constr_UseSafeMode_Set(bool value)
        {
            FileCacheService fileCache = new FileCacheService(useSafeMode: value);

            Assert.AreEqual(value, fileCache.UseSafeMode);
        }

        [Test]
        [TestCase(365)]
        [TestCase(1.75f)]
        [TestCase(1000)]
        public void Constr_DaysValid_Set(float value)
        {
            FileCacheService fileCache = new FileCacheService(daysValid: value);

            Assert.AreEqual(value, fileCache.DaysValid);
        }

        [Test]
        [TestCase(-365)]
        [TestCase(0)]
        [TestCase(-1.75f)]
        public void Constr_DaysValidNegativeOrZero_SetTo1(float value)
        {
            FileCacheService fileCache = new FileCacheService(daysValid: value);

            Assert.AreEqual(1, fileCache.DaysValid);
        }
    }
}
