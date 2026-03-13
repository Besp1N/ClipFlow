using System.Diagnostics;

namespace ClipFlow.Infrastructure.Download;

public sealed class YtDlpCommandBuilder
{
    public ProcessStartInfo Build(Uri uri, string outputDirectory)
    {
        var outTemplate = Path.Combine(outputDirectory, "%(uploader)s_%(id)s.%(ext)s");

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

        return startInfo;
    }
}