namespace ClipFlow.Application.UseCases.Download;

public sealed record DownloadClipRequest(
    string Url,
    string OutputDirectory);
    
