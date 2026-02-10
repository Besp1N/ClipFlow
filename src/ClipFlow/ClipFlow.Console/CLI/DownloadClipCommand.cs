namespace ClipFlow.Console.CLI;

public class DownloadClipCommand
{
    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("DownloadClipCommand executed.");
        return await Task.FromResult(0);
    }
}