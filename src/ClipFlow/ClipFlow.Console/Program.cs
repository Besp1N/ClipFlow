using ClipFlow.Console.CLI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddTransient<DownloadClipCommand>();

using var host = builder.Build();


var command = host.Services.GetRequiredService<DownloadClipCommand>();
return await command.RunAsync(args, cts.Token);
