using FakeItEasy;
using i5.Toolkit.Core.AppConsole;
using i5.Toolkit.Core.Editor.TestHelpers;
using NUnit.Framework;

namespace i5.Toolkit.Core.Tests.AppConsole
{
    /// <summary>
    /// Tests for the default log formatter
    /// </summary>
    public class DefaultLogFormatterTests
    {
        /// <summary>
        /// Resets the test scene
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            EditModeTestUtilities.ResetScene();
        }

        /// <summary>
        /// Checks that the log message's content is output as text
        /// </summary>
        [Test]
        public void Format_OutputIsLogContent()
        {
            const string expectedContent = "my test content";
            DefaultConsoleFormatter formatterLogic = new DefaultConsoleFormatter();

            ILogMessage logMessage = A.Fake<ILogMessage>();
            A.CallTo(() => logMessage.Content).Returns(expectedContent);

            string result = formatterLogic.Format(logMessage);

            Assert.AreEqual(expectedContent, result);
        }
    }
}
