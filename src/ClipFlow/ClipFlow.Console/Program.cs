using ClipFlow.Application.DependencyInjection;
using ClipFlow.Console.CLI;
using ClipFlow.Console.CLI.Root;
using ClipFlow.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
        levelSwitch: null,
        outputTemplate: "{Message:lj}{NewLine}",
        standardErrorFromLevel: Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.File(
        path: "logs/clipflow-.log",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
        outputTemplate:
        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

    .CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddSingleton<CliRoot>();
builder.Services.AddTransient<DownloadClipCommand>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

Console.WriteLine($"ENV: {builder.Environment.EnvironmentName}");

using var host = builder.Build();

var router = host.Services.GetRequiredService<CliRoot>();

var result = await router.RunAsync(args, cts.Token);
if (result.IsFailure)
{
    Console.Error.Write(result.ErrorMessage);
}
