using FakeItEasy;
using i5.Toolkit.Core.Editor.TestHelpers;
using i5.Toolkit.Core.VersionTool;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace i5.Toolkit.Core.Tests.VersionTool
{
    public class GitVersionCalculatorTests
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
        public void TryGetVersion_GitError_ReturnsFalse()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner.RunCommand(@"describe --tags --long --match ‘v[0–9]*’", out ignored, out ignored)).Returns(1).AssignsOutAndRefParameters("testOutput", "testError");

            Assert.IsFalse(gitVersion.TryGetVersion(out string version));
        }

        [Test]
        public void TryGetVersion_GitError_Version0_1()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner.RunCommand(@"describe --tags --long --match ‘v[0–9]*’", out ignored, out ignored)).Returns(1).AssignsOutAndRefParameters("testOutput", "testError");

            gitVersion.TryGetVersion(out string version);

            Assert.AreEqual("0.1.0", version);
        }

        [Test]
        public void TryGetVersion_GitError_LogsWarning()
        {

            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner.RunCommand(@"describe --tags --long --match ‘v[0–9]*’", out ignored, out ignored)).Returns(1).AssignsOutAndRefParameters("testOutput", "testError");

            LogAssert.Expect(LogType.Warning, "Error running git: testError");

            gitVersion.TryGetVersion(out string version);
        }

        [Test]
        public void TryGetVersion_GitGivesVersion_ReturnsTrue()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"describe --tags --long --match ‘v[0–9]*’",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters("v1.2-3-g4ab59cb78", "");

            Assert.IsTrue(gitVersion.TryGetVersion(out string version));
        }

        [Test]
        public void TryGetVersion_GitGivesVersion_VersionCorrect()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"describe --tags --long --match ‘v[0–9]*’",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters("v1.2-3-g4ab59cb78", "");

            gitVersion.TryGetVersion(out string version);

            Assert.AreEqual("1.2.3", version);
        }

        [Test]
        public void TryGetBranch_GitError_ReturnsFalse()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-parse --abbrev-ref HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            Assert.IsFalse(gitVersion.TryGetBranch(out string branch));
        }

        [Test]
        public void TryGetBranch_GitError_BranchNameUnknown()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-parse --abbrev-ref HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            gitVersion.TryGetBranch(out string branch);
            Assert.AreEqual("UNKNOWN", branch);
        }

        [Test]
        public void TryGetBranch_GitError_LogsWarning()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-parse --abbrev-ref HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            LogAssert.Expect(LogType.Warning, "Error running git: testError");

            gitVersion.TryGetBranch(out string branch);
        }

        [Test]
        public void TryGetBranch_GitGivesBranch_ReturnsTrue()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-parse --abbrev-ref HEAD",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters("features/myBranch", "");

            Assert.IsTrue(gitVersion.TryGetBranch(out string branch));
        }

        [Test]
        public void TryGetBranch_GitGivesBranch_BranchCorrect()
        {
            const string expectedbranchName = "features/myBranch";
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-parse --abbrev-ref HEAD",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters(expectedbranchName, "");

            gitVersion.TryGetBranch(out string branch);
            Assert.AreEqual(expectedbranchName, branch);
        }

        [Test]
        public void TryGetTotalCommitsOnBranch_GitError_ReturnsFalse()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-list --count HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            Assert.IsFalse(gitVersion.TryGetTotalCommitsOnBranch(out int number));
        }

        [Test]
        public void TryGetTotalCommitsOnBranch_GitError_Count0()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-list --count HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            gitVersion.TryGetTotalCommitsOnBranch(out int number);
            Assert.AreEqual(0, number);
        }

        [Test]
        public void TryGetTotalCommitsOnBranch_GitError_LogsWarning()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-list --count HEAD",
                out ignored, out ignored))
                .Returns(1)
                .AssignsOutAndRefParameters("testOutput", "testError");

            LogAssert.Expect(LogType.Warning, "Error running git: testError");
            gitVersion.TryGetTotalCommitsOnBranch(out int number);
        }

        [Test]
        public void TryGetTotalCommitsOnBranch_GitGivesCount_ReturnsTrue()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-list --count HEAD",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters("123", "");

            Assert.IsTrue(gitVersion.TryGetTotalCommitsOnBranch(out int number));
        }

        [Test]
        public void TryGetTotalCommitsOnBranch_GitGivesCount_CountCorrect()
        {
            GitVersionCalculator gitVersion = CreateGitVersionCalculator(out IGitRunner gitRunner);
            string ignored = null;
            A.CallTo(() => gitRunner
            .RunCommand(
                @"rev-list --count HEAD",
                out ignored, out ignored))
                .Returns(0)
                .AssignsOutAndRefParameters("123", "");

            gitVersion.TryGetTotalCommitsOnBranch(out int number);
            Assert.AreEqual(123, number);
        }

        private GitVersionCalculator CreateGitVersionCalculator(out IGitRunner fakeGitRunner)
        {
            GitVersionCalculator gitVersionCalculator = new GitVersionCalculator();
            fakeGitRunner = A.Fake<IGitRunner>();
            gitVersionCalculator.GetType()
                .GetField(
                "gitRunner",
                System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance)
                .SetValue(gitVersionCalculator, fakeGitRunner);
            return gitVersionCalculator;
        }
    }
}
