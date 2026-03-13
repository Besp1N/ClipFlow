using ClipFlow.Application.UseCases.Download;
using Microsoft.Extensions.DependencyInjection;

namespace ClipFlow.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<DownloadClipAsyncUseCase>();
        services.AddSingleton<DownloadClipRequestValidator>();

        return services;
    }
}