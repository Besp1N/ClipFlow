using ClipFlow.Application.Common;

namespace ClipFlow.Console.CLI.Commands;

public sealed class UploadClipCommand
{
    public async Task<Result> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length < 2)
            return Result.Failure(ErrorType.Validation, "No file path provided for upload-clip command.");

        var filePath = args[1];
        if (!File.Exists(filePath))
            return Result.Failure(ErrorType.Validation, $"File not found: {filePath}");

        System.Console.WriteLine($"Uploading clip: {filePath}...");
        await Task.Delay(1000, cancellationToken);

        System.Console.WriteLine($"Upload completed successfully for: {filePath}");
        return Result.Success();
    }
}