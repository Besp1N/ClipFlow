using ClipFlow.Application.Abstractions.Upload;
using ClipFlow.Application.Common;
using Microsoft.Playwright;

namespace ClipFlow.Infrastructure.Upload;

public sealed class TikTokClipUploader : IClipUploader
{
    public UploadServiceType ServiceType => UploadServiceType.TikTok;

    public async Task<Result> UploadAsync(string filePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
            return Result.Failure(ErrorType.Validation, $"File not found: {filePath}");

        try
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                Locale = "en-US"
            });
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://www.tiktok.com/login", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
            
            var cookiesBtn = page.Locator("button:has-text('Allow all')");
            try
            {
                await cookiesBtn.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 10000
                });

                await cookiesBtn.ClickAsync();
            }
            catch (TimeoutException)
            {
                return Result.Failure(
                    ErrorType.ExternalService,
                    "Cookies accept button has not been clicked.");
            }
            
            await Task.Delay(5000, cancellationToken); 
            
            return Result.Success();
        }
        catch (PlaywrightException ex) when (ex.Message.Contains("Executable doesn't exist", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(
                ErrorType.Infrastructure,
                "Playwright browser binaries are not installed. Run:\n" +
                "1) dotnet build ClipFlow.Console/ClipFlow.Console.csproj\n" +
                "2) ./ClipFlow.Console/bin/Debug/net10.0/playwright.sh install chromium");
        }
        catch (PlaywrightException ex)
        {
            return Result.Failure(ErrorType.ExternalService, $"Playwright error: {ex.Message}");
        }
    }
}

