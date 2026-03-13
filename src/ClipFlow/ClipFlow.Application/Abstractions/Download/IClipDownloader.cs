using ClipFlow.Application.Common;

namespace ClipFlow.Application.Abstractions.Download;

public interface IClipDownloader
{
    Task<Result> DownloadAsync(Uri uri, string outputDirectory, CancellationToken cancellationToken);
}