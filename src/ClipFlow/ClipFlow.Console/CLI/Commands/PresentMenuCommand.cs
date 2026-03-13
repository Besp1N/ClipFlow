using ClipFlow.Application.Common;

namespace ClipFlow.Console.CLI.Commands;

public sealed class PresentMenuCommand
{
    public Result Run()
    {
        System.Console.WriteLine("ClipFlow CLI - Available Commands:");
        System.Console.WriteLine("1. download-clip <URL> output-dir <DIRECTORY> - Download a clip from the specified URL to the given output directory.");
        System.Console.WriteLine("2. help - Display this menu.");
        return Result.Success();
    }
}