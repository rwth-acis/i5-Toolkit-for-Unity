using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace i5.Toolkit.TestUtilities
{
    public static class AsyncTest
    {
        public static IEnumerator WaitForTask(Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }
    }
}