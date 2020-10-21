using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.Core.TestHelpers
{
    /// <summary>
    /// Utilities for asynchronous tests
    /// </summary>
    public static class AsyncTest
    {
        /// <summary>
        /// Waits for the given task to complete
        /// Use this instead of await in Unit tests.
        /// Unity's NUnit version cannot handle Task methods to yield on await
        /// Hence, the asynchronous tests must be marked as UnityTests.
        /// Awaitable calls are replaced with an assignment to a Task object
        /// </summary>
        /// <param name="task">The task object to await</param>
        /// <returns></returns>
        public static IEnumerator WaitForTask(Task task)
        {
            // busy waiting in a co-routine style while the task is not yet completed
            while (!task.IsCompleted)
            {
                yield return null;
            }
            // if the task is faulted, an exception has occurred
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }
    }
}