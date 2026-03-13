using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Download;

public sealed class DownloadClipRequestValidator
{
    public Result Validate(DownloadClipRequest request, out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrWhiteSpace(request.Url))
            return Result.Failure(ErrorType.Validation,"Url is required.");
        
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out uri))
            return Result.Failure(ErrorType.Validation,"Url is invalid.");

        var outputDir = request.OutputDirectory;
        if (string.IsNullOrWhiteSpace(outputDir))
            return Result.Failure(ErrorType.ExternalService,"OutputDirectory is required.");

        return Result.Success();
    }
}