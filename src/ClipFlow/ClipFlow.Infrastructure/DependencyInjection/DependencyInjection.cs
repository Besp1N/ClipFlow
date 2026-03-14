using ClipFlow.Application.Abstractions.Download;
using ClipFlow.Application.Abstractions.Upload;
using ClipFlow.Infrastructure.Download;
using ClipFlow.Infrastructure.Upload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipFlow.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<YtDlpCommandBuilder>();
        services.AddScoped<IClipDownloader, YtDlpClipDownloader>();

        services.AddScoped<IClipUploader, TikTokClipUploader>();
        services.AddScoped<IClipUploaderResolver, ClipUploaderResolver>();

        return services;
    }
}