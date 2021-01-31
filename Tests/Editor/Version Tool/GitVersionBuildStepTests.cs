using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.Utilities.SystemAdapters;
using i5.Toolkit.Core.VersionTool;
using NUnit.Framework;
using System;

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
        public void ContainsPlaceholders_ContainsGitVersion_ReturnsTrue()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            Assert.IsTrue(buildStep.ContainsPlaceholder("$gitVersion"));
        }

        [Test]
        public void ContainsPlaceholders_ContainsBranch_ReturnsTrue()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            Assert.IsTrue(buildStep.ContainsPlaceholder("$gitBranch"));
        }

        [Test]
        public void ContainsPlaceholders_ContainsAppVersion_ReturnsTrue()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            Assert.IsTrue(buildStep.ContainsPlaceholder("$appVersion"));
        }

        [Test]
        public void ContainsPlaceholders_NoPlaceholders_ReturnsFalse()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            Assert.IsFalse(buildStep.ContainsPlaceholder("$noPlaceholder"));
        }

        [Test]
        public void ContainsPlaceholders_Empty_ReturnsFalse()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            Assert.IsFalse(buildStep.ContainsPlaceholder(""));
        }

        [Test]
        public void ReplacePlaceholders_ContainsGitVersionPlaceholder_Replaces()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

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
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

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
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

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
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

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
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

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
        public void ReplacePlaceholders_AppVersion_EnvVarSet_UsesEnvVar()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            string ignored = null;
            A.CallTo(()
                => versionCalculator
                .TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.1.0");

            A.CallTo(()
                => systemEnvironment
                .GetEnvironmentVariable(A<string>.Ignored))
                .Returns("1.2.3");

            string result = buildStep.ReplacePlaceholders("prefix$appVersion-postfix");

            Assert.AreEqual("prefix1.2.3-postfix", result);
        }

        [Test]
        public void ReplacePlaceholders_AppVersion_EnvVarNotSet_UsesGit()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            string ignored = null;
            A.CallTo(()
                => versionCalculator
                .TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");

            A.CallTo(()
                => systemEnvironment
                .GetEnvironmentVariable(A<string>.Ignored))
                .Returns("");

            string result = buildStep.ReplacePlaceholders("prefix$appVersion-postfix");

            Assert.AreEqual("prefix1.2.3-postfix", result);
        }

        [Test]
        public void WSAVersion_VarEmptyGitGiven_ReturnsVersionUsingGit()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            string ignored = null;
            A.CallTo(() => versionCalculator.TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.2.3");
            A.CallTo(() => systemEnvironment.GetEnvironmentVariable(A<string>.Ignored))
                .Returns("");

            Version result = buildStep.WSAVersion;

            Assert.AreEqual(1, result.Major);
            Assert.AreEqual(2, result.Minor);
            Assert.AreEqual(3, result.Build);
            Assert.AreEqual(0, result.Revision);
        }

        [Test]
        public void WSAVersion_VarVersionGiven_ReturnsVarVersion()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            string ignored = null;
            A.CallTo(() => versionCalculator.TryGetVersion(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters("1.0.42");
            A.CallTo(() => systemEnvironment.GetEnvironmentVariable(A<string>.Ignored))
                .Returns("1.2.3");

            Version result = buildStep.WSAVersion;

            Assert.AreEqual(1, result.Major);
            Assert.AreEqual(2, result.Minor);
            Assert.AreEqual(3, result.Build);
            Assert.AreEqual(0, result.Revision);
        }

        [Test]
        public void AndroidVersion_EnvVarNotSet_UsesCommitCount()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            int ignored = 0;
            A.CallTo(() 
                => versionCalculator
                .TryGetTotalCommitsOnBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters(123);

            A.CallTo(()
                => systemEnvironment
                .GetEnvironmentVariable(A<string>.Ignored))
                .Returns("");

            int result = buildStep.AndroidVersion;

            Assert.AreEqual(123, result);
        }

        [Test]
        public void AndroidVersion_EnvVarSet_UsesEnvVar()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            int ignored = 0;
            A.CallTo(()
                => versionCalculator
                .TryGetTotalCommitsOnBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters(42);

            A.CallTo(()
                => systemEnvironment
                .GetEnvironmentVariable(A<string>.Ignored))
                .Returns("123");

            int result = buildStep.AndroidVersion;

            Assert.AreEqual(123, result);
        }

        [Test]
        public void AndroidVersion_EnvVarSetWrong_UsesCommitCount()
        {
            GitVersionBuildStep buildStep = CreateGitVersionBuildStep(
                out IGitVersionCalculator versionCalculator,
                out ISystemEnvironment systemEnvironment);

            int ignored = 0;
            A.CallTo(()
                => versionCalculator
                .TryGetTotalCommitsOnBranch(out ignored))
                .Returns(true)
                .AssignsOutAndRefParameters(123);

            A.CallTo(()
                => systemEnvironment
                .GetEnvironmentVariable(A<string>.Ignored))
                .Returns("test");

            int result = buildStep.AndroidVersion;

            Assert.AreEqual(123, result);
        }

        private GitVersionBuildStep CreateGitVersionBuildStep(
            out IGitVersionCalculator fakeGitVersion,
            out ISystemEnvironment fakeEnvironment)
        {
            GitVersionBuildStep buildStep = new GitVersionBuildStep();
            fakeGitVersion = A.Fake<IGitVersionCalculator>();
            fakeEnvironment = A.Fake<ISystemEnvironment>();
            buildStep.GetType()
                .GetField(
                "gitVersion",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance)
                .SetValue(buildStep, fakeGitVersion);

            buildStep.GetType()
                .GetField(
                "systemEnvironment",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance)
                .SetValue(buildStep, fakeEnvironment);
            return buildStep;
        }
    }
}
