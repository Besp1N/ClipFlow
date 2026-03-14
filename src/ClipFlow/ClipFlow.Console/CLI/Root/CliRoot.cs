using ClipFlow.Application.Common;
using ClipFlow.Console.CLI.Commands;

namespace ClipFlow.Console.CLI.Root;

public class CliRoot(
    DownloadClipCommand downloadClipCommand,
    PresentMenuCommand presentMenuCommand,
    UploadClipCommand uploadClipCommand)
{
    public async Task<Result> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        if (args.Length == 0)
            return Result.Failure(ErrorType.Validation, "No command provided. Use 'help' to see available commands.");

        var command = args[0].ToLowerInvariant();

        return command switch
        {
            "download-clip" => await downloadClipCommand.RunAsync(args, cancellationToken),
            "upload-clip" => await uploadClipCommand.RunAsync(args, cancellationToken),
            "help" => presentMenuCommand.Run(),
            _ => Result.Failure(ErrorType.Validation, "Invalid command provided. Use 'help' to see available commands.")
        };
    }
}