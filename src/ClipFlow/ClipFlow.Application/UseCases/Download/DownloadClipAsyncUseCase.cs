using ClipFlow.Application.Common;
using ClipFlow.Infrastructure.Twitch;
using Microsoft.Extensions.Options;

namespace ClipFlow.Application.UseCases.Download;

public class DownloadClipAsyncUseCase(IOptions<TwitchOptions> twitchOptions)
{
    public async Task<Result> ExecuteAsync(DownloadClipRequest request, CancellationToken cancellationToken)
    {
        var slug = new Uri(request.Url).Segments.LastOrDefault()?.TrimEnd('?').Replace("/", "") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure(ErrorType.Validation, "This in not a clip URL.");
        
        return await Task.FromResult(Result.Success());
    }
}