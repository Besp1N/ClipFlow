using ClipFlow.Application.UseCases.Download;
using ClipFlow.Application.UseCases.Upload;
using Microsoft.Extensions.DependencyInjection;

namespace ClipFlow.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<DownloadClipAsyncUseCase>();
        services.AddSingleton<DownloadClipRequestValidator>();

        services.AddScoped<UploadClipAsyncUseCase>();
        services.AddSingleton<UploadClipRequestValidator>();

        return services;
    }
}