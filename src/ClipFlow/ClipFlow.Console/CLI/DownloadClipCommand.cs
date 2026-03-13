using ClipFlow.Application.Common;
using ClipFlow.Application.UseCases.Download;

namespace ClipFlow.Console.CLI;

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

        return await downloadClipAsync.ExecuteAsync(downloadClipRequest, cancellationToken);
    }
}