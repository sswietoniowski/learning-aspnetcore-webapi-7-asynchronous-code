using Covers.Api;
using Covers.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ICoversService, FakeCoversService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMinimalApiEndpoints();

app.Run();
