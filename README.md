# Learning ASP.NET Core - WebAPI (.NET 6) Asynchronous Code

Based on this course [Developing an Asynchronous ASP.NET Core 6 Web API](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-developing-asynchronous/table-of-contents) :+1:.

Original course materials can be found [here](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-web-api-developing-asynchronous/exercise-files).

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
