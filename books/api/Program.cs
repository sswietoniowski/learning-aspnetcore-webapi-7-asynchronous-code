using Books.Api.Configurations.Extensions;
using Books.Api.DataAccess;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Throttle the thread pool (set available threads to amount of processors)
// WARNING! THESE SETTINGS ARE HERE ONLY FOR DEMONSTRATION PURPOSES
ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

builder.AddLogging();
builder.AddPersistence();
builder.AddMapper();
builder.AddHttpClient();
builder.AddGlobalErrorHandler();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalErrorHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await UpdateDatabase(app);

app.Run();

static async Task UpdateDatabase(WebApplication app)
{
    const int MAX_RETRIES = 3;
    const int RETRY_DELAY_IN_SECONDS = 5;
    var retryPolicy = Policy.Handle<Exception>()
        .WaitAndRetryAsync(retryCount: MAX_RETRIES,
            sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(RETRY_DELAY_IN_SECONDS));

    await retryPolicy.ExecuteAsync(async () =>
    {
        await app.SeedAsync();
    });
}