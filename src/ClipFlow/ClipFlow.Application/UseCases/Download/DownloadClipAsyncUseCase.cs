using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using ClipFlow.Application.Common;

namespace ClipFlow.Application.UseCases.Download;

public sealed class DownloadClipAsyncUseCase
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(10);

    public async Task<Result> ExecuteAsync(DownloadClipRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Url))
            return Result.Failure(ErrorType.Validation,"Url is required.");
        
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri))
            return Result.Failure(ErrorType.Validation,"Url is invalid.");

        var outputDir = request.OutputDirectory;
        if (string.IsNullOrWhiteSpace(outputDir))
            return Result.Failure(ErrorType.ExternalService,"OutputDirectory is required.");

        try
        {
            Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorType.ExternalService,$"Cannot create output directory '{outputDir}': {ex.Message}");
        }
        
        var outTemplate = Path.Combine(outputDir, "%(uploader)s_%(id)s.%(ext)s");
        
        var ytDlpArguments = new[]
        {
            "--no-part",
            "--no-playlist",
            "--newline",
            "-o", outTemplate,
            uri.ToString()
        };

        var startInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in ytDlpArguments)
            startInfo.ArgumentList.Add(argument);

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
                return Result.Failure(ErrorType.ExternalService,"Failed to start yt-dlp process. Is yt-dlp installed and available in PATH?");

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
                return Result.Failure(ErrorType.Unexpected,$"yt-dlp timed out after {DefaultTimeout.TotalMinutes:N0} minutes.\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
            }
            
            await Task.Delay(50, cancellationToken);

            if (process.ExitCode == 0)
                return Result.Success();
            
            var hint = process.ExitCode == 2
                ? "Hint: ensure yt-dlp is installed and accessible (PATH), and try updating yt-dlp."
                : "Hint: try updating yt-dlp (they ship fixes for Twitch frequently).";

            return Result.Failure(ErrorType.Unexpected,$"yt-dlp failed with exit code {process.ExitCode}.\n{hint}\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");

        }
        catch (Win32Exception ex)
        {
            return Result.Failure(ErrorType.Infrastructure,$"Cannot execute yt-dlp: {ex.Message}. Is yt-dlp installed and in PATH?");
        }
        catch (Exception ex)
        {
            TryKill(process);
            return Result.Failure(ErrorType.Unexpected,$"Unexpected error: {ex.Message}\nSTDOUT:\n{stdout}\nSTDERR:\n{stderr}");
        }
    }

    private static void TryKill(Process p)
    {
        try
        {
            if (!p.HasExited) p.Kill(entireProcessTree: true);
        }
        catch
        {
            ;
        }
    }
}
