using ClipFlow.Application.Abstractions.Upload;
using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Upload;

public sealed class UploadClipAsyncUseCase(
    UploadClipRequestValidator uploadClipRequestValidator,
    IClipUploaderResolver clipUploaderResolver)
{
    public async Task<Result> ExecuteAsync(UploadClipRequest request, CancellationToken cancellationToken)
    {
        var validationResult = uploadClipRequestValidator.Validate(request);
        if (validationResult.IsFailure)
            return validationResult;

        var uploaderResolutionResult = clipUploaderResolver.Resolve(request.ServiceType);
        if (uploaderResolutionResult.IsFailure || uploaderResolutionResult.Value is null)
        {
            return Result.Failure(
                uploaderResolutionResult.ErrorType,
                uploaderResolutionResult.ErrorMessage ?? "Cannot resolve uploader.");
        }

        return await uploaderResolutionResult.Value.UploadAsync(request.FilePath, cancellationToken);
    }
}