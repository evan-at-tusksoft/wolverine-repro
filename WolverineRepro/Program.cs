using JasperFx.Core;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Oakton;
using Oakton.Resources;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Postgresql;
using WolverineRepro;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new Exception("Database connection string not found!");
builder.Services.AddDbContextWithWolverineIntegration<WolverineReproDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Host.UseWolverine((context, options) =>
{
    options.PersistMessagesWithPostgresql(connectionString, "public");

    options.UseEntityFrameworkCoreTransactions();

    options.Policies.AutoApplyTransactions();
    options.Policies.UseDurableLocalQueues();
    options.Policies.OnException<NpgsqlException>()
        .RetryWithCooldown(50.Milliseconds(), 100.Milliseconds(), 250.Milliseconds());
});
builder.Host.UseResourceSetupOnStartup();
builder.Host.ApplyOaktonExtensions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/sendmessage", async (IMessageBus bus) =>
{
    await bus.SendAsync(new TestMessage());
})
.WithOpenApi();

await app.RunOaktonCommands(args);
