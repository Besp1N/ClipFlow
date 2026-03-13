using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using ClipFlow.Application.Abstractions.Download;
using ClipFlow.Application.Common;

namespace ClipFlow.Infrastructure.Download;

public sealed class YtDlpClipDownloader(YtDlpCommandBuilder ytDlpCommandBuilder) : IClipDownloader
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(10);

    public async Task<Result> DownloadAsync(Uri uri, string outputDirectory, CancellationToken cancellationToken)
    {
        try
        {
            Directory.CreateDirectory(outputDirectory);
        }
        catch (Exception ex)
        {
            return Result.Failure(
                ErrorType.ExternalService,
                $"Cannot create output directory '{outputDirectory}': {ex.Message}");
        }

        var startInfo = ytDlpCommandBuilder.Build(uri, outputDirectory);
        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        using var process = new Process();
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                stdout.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data is not null)
                stderr.AppendLine(e.Data);
        };

        try
        {
            if (!process.Start())
            {
                return Result.Failure(
                    ErrorType.ExternalService,
                    "Failed to start yt-dlp process. Is yt-dlp installed and available in PATH?");
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(DefaultTimeout);

            try
            {
                await process.WaitForExitAsync(timeoutCts.Token);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                TryKill(process);
                return Result.Failure(
                    ErrorType.Unexpected,
                    $"yt-dlp timed out after {DefaultTimeout.TotalMinutes:N0} minutes.\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
            }

            await Task.Delay(50, cancellationToken);

            if (process.ExitCode == 0)
                return Result.Success();

            var hint = process.ExitCode == 2
                ? "Hint: ensure yt-dlp is installed and accessible (PATH), and try updating yt-dlp."
                : "Hint: try updating yt-dlp (they ship fixes for Twitch frequently).";

            return Result.Failure(
                ErrorType.Unexpected,
                $"yt-dlp failed with exit code {process.ExitCode}.\n{hint}\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
        }
        catch (Win32Exception ex)
        {
            return Result.Failure(
                ErrorType.Infrastructure,
                $"Cannot execute yt-dlp: {ex.Message}. Is yt-dlp installed and in PATH?");
        }
        catch (Exception ex)
        {
            TryKill(process);
            return Result.Failure(
                ErrorType.Unexpected,
                $"Unexpected error: {ex.Message}\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
        }
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
        }
        catch
        {
            ;
        }
    }
}