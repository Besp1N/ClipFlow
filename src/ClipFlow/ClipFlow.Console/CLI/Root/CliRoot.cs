using ClipFlow.Application.Common;

namespace ClipFlow.Console.CLI.Root;

public class CliRoot(DownloadClipCommand downloadClipCommand)
{
    public async Task<Result> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length == 0)
            return Result.Failure(ErrorType.Validation, "No command provided.");

        var command = args[0].ToLowerInvariant();

        return command switch
        {
            "download-clip" => await downloadClipCommand.RunAsync(args, cancellationToken),
            _ => throw new ArgumentException("Unknown command provided.")
        };
    }
}