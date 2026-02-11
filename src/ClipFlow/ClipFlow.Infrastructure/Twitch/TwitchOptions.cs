namespace ClipFlow.Infrastructure.Twitch;

public class TwitchOptions
{
    public const string SectionName = "Twitch";

    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
    public string AccessToken { get; init; } = null!;
}