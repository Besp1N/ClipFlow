using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Upload;

public sealed class UploadClipRequestValidator
{
    private readonly string[] _supportedFileExtensions = [".mp4"];
    
    public Result Validate(UploadClipRequest request)
    {
        if (!Enum.IsDefined(request.ServiceType))
            return Result.Failure(ErrorType.Validation, "Provided upload service is not supported.");

        if (!File.Exists(request.FilePath))
            return Result.Failure(ErrorType.Validation,"Provided file does not exists.");
        
        var fileExtension = Path.GetExtension(request.FilePath).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(fileExtension) || !_supportedFileExtensions.Contains(fileExtension))
            return Result.Failure(ErrorType.Validation,"Unsupported file type. Supported types are: " + string.Join(", ", _supportedFileExtensions));
        
        return Result.Success();
    }
}