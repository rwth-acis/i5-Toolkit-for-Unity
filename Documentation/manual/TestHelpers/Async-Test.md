# Async Test

## Use Case

When writing unit tests, you will encounter tests where [asynchronous methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/) need to be awaited.
With the NUnit version that is used in Unity, you can only create test methods that are either `async void` or `IEnumerator` coroutines for asynchronous processes.
The usual way of awaiting a method call in an `async Task` method is not possible.
You should not use `async void` methods as it can lead to unexpected behavior, especially if an exception is thrown (for a detailed explanation on why you should avoid `async void`, click [here](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming#avoid-async-void)).
Therefore, the i5 Toolkit provides an <xref:i5.Toolkit.Core.TestHelpers.AsyncTest> class.
With the help of this class, the async methods are awaited inside of coroutines.

## Usage

### Async Methods Without Return Value

In this example, a test must await the async method `FooAsync()`.
Create a `[UnityTest]` unit test which can run over multiple frames.
This means that the return type of this test method needs to be `IEnumerator`.
After that write the test.
Instead of adding `await FooAsync()`, first create a <xref:System.Threading.Tasks.Task> object from the method call.
 After that, use <xref:i5.Toolkit.Core.TestHelpers.AsyncTest.WaitForTask*> to execute the task:

```[C#]
[UnityTest]
public IEnumerator MyAsyncUnitTest()
{
    ...
    // first create a task object
    // note that this does not execute the method directly
    Task task = FooAsync();
    // execute the task inside of the WaitForTask coroutine
    yield return AsyncTest.WaitForTask(task);

    Debug.Log("This log is only printed once the async task has finished");
}
```

### Async Methods With Return Value

Write the test in the same way as in the example without a return value.
Instead of using <xref:System.Threading.Tasks.Task>, use the variant <xref:System.Threading.Tasks.Task`1>.
This class takes a generic type which defines the result's type.
After the completion of a task, the task object contains the result in its <xref:System.Threading.Tasks.Task`1.Result> property.

```[C#]
[UnityTest]
public IEnumerator MyAsyncUnitTest()
{
    ...
    // first create a task object
    // note that this does not execute the method directly
    Task<int> task = FooReturnValueAsync();
    // execute the task inside of the WaitForTask coroutine
    yield return AsyncTest.WaitForTask(task);

    Debug.Log("The result is: " + task.Result);
}
```

## Functionality

<xref:i5.Toolkit.Core.TestHelpers.AsyncTest.WaitForTask*> executes the given task in a coroutine and waits for it to complete.
After that, the result is available in the task object.
The coroutine of the test function waits for the <xref:i5.Toolkit.Core.TestHelpers.AsyncTest.WaitForTask*> method to finish.
If an exception is thrown during the execution of the async method, <xref:i5.Toolkit.Core.TestHelpers.AsyncTest.WaitForTask*> will re-throw this exception.
This cirumvents the problem that exceptions in coroutines occur silently.