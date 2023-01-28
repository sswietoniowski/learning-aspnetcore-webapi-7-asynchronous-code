# Learning ASP.NET Core - WebAPI (.NET 6) Asynchronous Code

Based on this course [Developing an Asynchronous ASP.NET Core 6 Web API](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-developing-asynchronous/table-of-contents) :+1:.

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-developing-asynchronous/exercise-files).

## Benefits of Asynchronous Programming

- **scalability** (scaling up) - asynchronous programming allows you to perform multiple operations at the same time. This can improve the scalability of your application,
- asynchronous code might or might not be **faster** than synchronous code. It depends on the operation and the context. For example, if you are performing a long-running operation, then asynchronous code might be faster than synchronous code. However, if you are performing a short-running operation, then synchronous code might be faster than asynchronous code.

We must understand our workload and the context in which we are executing our code. We must also understand the performance characteristics of the operations we are performing.

Our code can be I/O-bound or CPU-bound. I/O-bound code is code that is waiting for an I/O operation to complete. CPU-bound code is code that is performing a CPU-intensive operation. In case of I/O-bound code, asynchronous code might be faster than synchronous code. In case of CPU-bound code (e.g. a long-running loop), synchronous code might be faster than asynchronous code. While developing desktop applications we can use background threads to perform CPU-bound operations and while doing so we can keep the UI responsive. However, in case of web applications this idea might lead to be as useful and we should avoid it of course we can still use asynchronous code to perform I/O-bound operations.

## Async Patterns

There are three main patterns for asynchronous programming in .NET:

- **TAP** (_Task-based Asynchronous Pattern_) is **the recommended pattern** for asynchronous programming in .NET,
- **EAP** (_Event-based Asynchronous Pattern_) is the legacy pattern for asynchronous programming in .NET,
- **APM** (_Asynchronous Programming Model_) is the legacy pattern for asynchronous programming in .NET.

## TAP

> The TAP is the recommended pattern for asynchronous programming in .NET. It is based on the `Task` and `Task<T>` types. The `Task` type represents a single operation that does not return a value. The `Task<T>` type represents a single operation that returns a value of type `T`.

Example:

```csharp
public async Task<string> GetAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}
```

## EAP

> The EAP is the legacy pattern for asynchronous programming in .NET. It is based on events. The `XXXCompleted` events are raised when an asynchronous operation completes. The `XXXCompletedEventArgs` class contains the result of the asynchronous operation. The `XXXAsync` methods start an asynchronous operation.

Example:

```csharp
public void GetAsync(string url, Action<string> callback)
{
    var client = new WebClient();
    client.DownloadStringCompleted += (sender, e) => callback(e.Result);
    client.DownloadStringAsync(new Uri(url));
}
```

## APM

> The APM is the legacy pattern for asynchronous programming in .NET. It is based on the `BeginXXX` and `EndXXX` methods. The `BeginXXX` methods start an asynchronous operation and return an `IAsyncResult` object. The `EndXXX` methods complete an asynchronous operation and return a value.

Example using `BeginXXX` and `EndXXX` methods:

```csharp
public string GetAsync(string url)
{
    var client = new WebClient();
    var result = client.BeginDownloadString(new Uri(url), null, null);
    return client.EndDownloadString(result);
}
```

## Async Return Types

There are three main return types for asynchronous programming in .NET:

- `void` - the method does not return a value (not recommended - only advised for event handlers),
- `Task` - the method returns a `Task` object that represents a single operation that does not return a value,
- `Task<T>` - the method returns a `Task<T>` object that represents a single operation that returns a value of type `T`.

`Task` and `Task<T>` represents the execution of an asynchronous operation. They are not the result of an asynchronous operation - which is returned by the `Result` property (if the operation completed successfully) or the `Exception` property (if the operation failed).

`Task` class has some useful properties:

- `IsCompleted` - indicates whether the task has completed,
- `IsCanceled` - indicates whether the task has been canceled,
- `IsFaulted` - indicates whether the task has faulted,
- `Status` - indicates the status of the task.
