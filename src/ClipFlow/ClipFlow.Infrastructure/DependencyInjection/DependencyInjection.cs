using ClipFlow.Application.Abstractions.Download;
using ClipFlow.Infrastructure.Download;
using ClipFlow.Infrastructure.Twitch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClipFlow.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<TwitchOptions>()
            .Bind(configuration.GetSection(TwitchOptions.SectionName))
            .Validate(o =>
                    !string.IsNullOrWhiteSpace(o.ClientId) &&
                    !string.IsNullOrWhiteSpace(o.AccessToken),
                "Twitch configuration is invalid. Required: Twitch:ClientId, Twitch:AccessToken.")
            .ValidateOnStart();

        services.AddHttpClient("Twitch", client =>
        {
            client.BaseAddress = new Uri("https://api.twitch.tv/helix/");
        });

        services.AddSingleton<YtDlpCommandBuilder>();
        services.AddScoped<IClipDownloader, YtDlpClipDownloader>();

        return services;
    }
}