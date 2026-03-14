using ClipFlow.Application.Common;

namespace ClipFlow.Application.Abstractions.Upload;

public interface IClipUploader
{
    UploadServiceType ServiceType { get; }

    Task<Result> UploadAsync(string filePath, CancellationToken cancellationToken);
}