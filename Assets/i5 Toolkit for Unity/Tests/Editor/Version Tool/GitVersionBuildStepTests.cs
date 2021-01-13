using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.VersionTool;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.VersionTool
{
    public class GitVersionBuildStepTests
    {
        /// <summary>
        /// Resets the scene
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        [Test]
        public void ContainsPlaceholders_ContainsVersion_ReturnsTrue()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Assert.IsTrue(buildStep.ContainsPlaceholder("$gitVersion"));
        }

        [Test]
        public void ContainsPlaceholders_ContainsBranch_ReturnsTrue()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Assert.IsTrue(buildStep.ContainsPlaceholder("$gitBranch"));
        }

        [Test]
        public void ContainsPlaceholders_NoPlaceholders_ReturnsFalse()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Assert.IsFalse(buildStep.ContainsPlaceholder("$noPlaceholder"));
        }

        [Test]
        public void ContainsPlaceholders_Empty_ReturnsFalse()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Assert.IsFalse(buildStep.ContainsPlaceholder(""));
        }

        [Test]
        public void ReplacePlaceholders_ContainsVersionPlaceholder_Replaces()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            string ignored = null;
            A.CallTo(()
                => versionCalculator
                .TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");

            string result = buildStep.ReplacePlaceholders("prefix$gitVersion-postfix");

            Assert.AreEqual("prefix1.2.3-postfix", result);
        }

        [Test]
        public void ReplacePlaceholders_ContainsVersionTwice_ReplacesBoth()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            string ignored = null;
            A.CallTo(()
                => versionCalculator
                .TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");

            string result = buildStep.ReplacePlaceholders("prefix$gitVersion-infix-$gitVersion-postfix");

            Assert.AreEqual("prefix1.2.3-infix-1.2.3-postfix", result);
        }

        [Test]
        public void ReplacePlaceholders_ContainsNothing_NoModifications()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            string ignored = null;
            A.CallTo(()
                => versionCalculator
                .TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");

            string result = buildStep.ReplacePlaceholders("no-placeholder");

            Assert.AreEqual("no-placeholder", result);
        }

        [Test]
        public void ReplacePlaceholders_ContainsBranch_ReplacesBranch()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            string ignored = null;
            A.CallTo(()
                => versionCalculator.TryGetBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("features/testBranch");

            string result = buildStep.ReplacePlaceholders("prefix-$gitBranch-postfix");

            Assert.AreEqual("prefix-features/testBranch-postfix", result);
        }

        [Test]
        public void ReplacePlaceholders_ContainsVersionAndBranch_ReplacesBoth()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            string ignored = null;
            A.CallTo(()
                => versionCalculator.TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");
            A.CallTo(()
                => versionCalculator.TryGetBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("features/testBranch");

            string result = buildStep.ReplacePlaceholders("v$gitVersion-$gitBranch");

            Assert.AreEqual("v1.2.3-features/testBranch", result);
        }

        [Test]
        public void WSAVersion_IsolatedVersionGiven_ReturnsVersion()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Version result = buildStep.WSAVersion("1.2.3");

            Assert.AreEqual(1, result.Major);
            Assert.AreEqual(2, result.Minor);
            Assert.AreEqual(3, result.Build);
            Assert.AreEqual(0, result.Revision);
        }

        [Test]
        public void WSAVersion_InfixVersionGiven_ReturnsVersion()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Version result = buildStep.WSAVersion("prefix1.2.3postfix");

            Assert.AreEqual(1, result.Major);
            Assert.AreEqual(2, result.Minor);
            Assert.AreEqual(3, result.Build);
            Assert.AreEqual(0, result.Revision);
        }

        [Test]
        public void WSAVersion_InvalidString_ReturnsDefault()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);

            Version result = buildStep.WSAVersion("no-version");

            Assert.AreEqual(0, result.Major);
            Assert.AreEqual(0, result.Minor);
            Assert.AreEqual(1, result.Build);
            Assert.AreEqual(0, result.Revision);
        }

        [Test]
        public void AndroidVersion_UsesCommitCount()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(out IGitVersionCalculator versionCalculator);
            int ignored = 0;
            A.CallTo(() 
                => versionCalculator
                .TryGetTotalCommitsOnBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters(123);

            int result = buildStep.AndroidVersion();

            Assert.AreEqual(123, result);
        }

        private GitVersionBuildStep CreateGitVersionBuildStep(out IGitVersionCalculator fakeGitVersion)
        {
            GitVersionBuildStep buildStep = new GitVersionBuildStep();
            fakeGitVersion = A.Fake<IGitVersionCalculator>();
            buildStep.GetType()
                .GetField(
                "gitVersion",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance)
                .SetValue(buildStep, fakeGitVersion);
            return buildStep;
        }
    }
}
