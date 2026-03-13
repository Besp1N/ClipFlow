using ClipFlow.Application.Common;
using ClipFlow.Application.UseCases.Download;

namespace ClipFlow.Console.CLI.Commands;

public class DownloadClipCommand(DownloadClipAsyncUseCase downloadClipAsync)
{
    public async Task<Result> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length < 2)
            return Result.Failure(ErrorType.Validation, "No URL provided for download-clip command.");

        var outputDirIndex = Array.IndexOf(args, "output-dir");
        if (outputDirIndex == -1 || outputDirIndex + 1 >= args.Length)
            return Result.Failure(ErrorType.Validation, "No output directory provided for download-clip command.");

        var downloadClipRequest = new DownloadClipRequest(Url: args[1], OutputDirectory: args[outputDirIndex + 1]);

        System.Console.WriteLine($"Downloading clip: {downloadClipRequest.Url}...");
        var result = await downloadClipAsync.ExecuteAsync(downloadClipRequest, cancellationToken);
        if (result.IsSuccess)
        {
            System.Console.WriteLine($"Download completed successfully. Clip saved to: {downloadClipRequest.OutputDirectory}");
            return Result.Success();
        }

        System.Console.WriteLine($"Error downloading clip: {result.ErrorMessage}");
        return result;
    }
}