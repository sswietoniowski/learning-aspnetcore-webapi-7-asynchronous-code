using Books.Api.Configurations.Extensions;
using Books.Api.DataAccess;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPersistence();
builder.AddGlobalErrorHandler();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalErrorHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

const int MAX_RETRIES = 3;
const int RETRY_DELAY_IN_SECONDS = 5;
var retryPolicy = Policy.Handle<Exception>()
	.WaitAndRetryAsync(retryCount: MAX_RETRIES, 
        sleepDurationProvider: (attemptCount) => TimeSpan.FromSeconds(RETRY_DELAY_IN_SECONDS));

await retryPolicy.ExecuteAsync(async () =>
{
    await app.SeedAsync();
});

app.Run();
