using ClipFlow.Application.Abstractions.Download;
using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Download;

public sealed class DownloadClipAsyncUseCase(
    DownloadClipRequestValidator validator,
    IClipDownloader clipDownloader)
{
    public async Task<Result> ExecuteAsync(DownloadClipRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request, out var uri);
        if (validationResult.IsFailure || uri is null)
            return validationResult;

        return await clipDownloader.DownloadAsync(uri, request.OutputDirectory, cancellationToken);
    }
}
