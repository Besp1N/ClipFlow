using System.IO.Enumeration;
using ClipFlow.Application.Common;
using ClipFlow.Application.UseCases.Upload;

namespace ClipFlow.Console.CLI.Commands;

public sealed class UploadClipCommand(UploadClipAsyncUseCase uploadClipAsyncUseCase)
{
    public async Task<Result> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length < 2)
            return Result.Failure(ErrorType.Validation, "No file path provided for upload-clip command.");

        var filePath = args[1];
        if (!File.Exists(filePath))
            return Result.Failure(ErrorType.Validation, $"File not found: {filePath}");

        var uploadServiceTypeIndex = Array.IndexOf(args, "--upload-service");
        if (uploadServiceTypeIndex == -1 ||  uploadServiceTypeIndex + 1 >= args.Length)
            return Result.Failure(ErrorType.Validation, $"Upload service not been provided. Use --upload-service flag.");
        
        if (!Enum.TryParse<UploadServiceType>(args[uploadServiceTypeIndex + 1], true, out var uploadServiceType) || !Enum.IsDefined(uploadServiceType))
            return Result.Failure(ErrorType.Validation, $"Provides service is not supported. Supported services: {string.Join(", ", Enum.GetNames<UploadServiceType>())}");
        
        var uploadRequest = new UploadClipRequest(FilePath: filePath, ServiceType: uploadServiceType);
        
        var uploadResult = await uploadClipAsyncUseCase.ExecuteAsync(uploadRequest, cancellationToken);
        if (uploadResult.IsSuccess)
            System.Console.WriteLine($"Upload completed successfully.");
        
        return uploadResult;
    }
}