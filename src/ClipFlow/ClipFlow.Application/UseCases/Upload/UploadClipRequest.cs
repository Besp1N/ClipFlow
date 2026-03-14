using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Upload;

public sealed record UploadClipRequest(
    string FilePath,
    UploadServiceType ServiceType);