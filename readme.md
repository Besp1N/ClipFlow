# ClipFlow v_0.0.1

ClipFlow is a .NET CLI application for working with short-form video clips:
- downloading clips from a URL (currently via `yt-dlp`),
- preparing uploads to social platforms (currently `TikTok` via Playwright - prototype stage).

The solution follows a layered architecture (`Console`, `Application`, `Infrastructure`, `Domain`, `Tests`) and is designed to make adding new upload providers straightforward.

## Current Features

- `download-clip` - downloads media from a URL to a selected output directory.
- `upload-clip` - launches a Playwright browser and runs a basic TikTok test flow (open page + accept cookies).
- `help` - prints available CLI commands.

> Note: TikTok upload is currently a prototype and does not publish a final post yet.

## Requirements

- macOS / Linux / Windows
- .NET SDK 10.0
- `yt-dlp` available in `PATH` (for `download-clip`)
- Playwright Chromium binaries (for `upload-clip`)

## Quick Start

```bash
cd ./src/ClipFlow
dotnet restore
dotnet build
```

Run tests:

```bash
dotnet test
```

## Playwright Setup (Chromium)

After building, install the Playwright browser binaries from the generated package directory:

```bash
cd ./ClipFlow.Infrastructure/bin/Debug/net10.0/.playwright/package
node ./cli.js install chromium
```

If your target framework or build configuration differs, adjust the path accordingly (for example `Release` instead of `Debug`).

## CLI Usage

### 1) Help

```bash
dotnet run --project ClipFlow.Console -- help
```

### 2) Download a clip

```bash
dotnet run --project ClipFlow.Console -- download-clip "https://www.twitch.tv/videos/123456789" --output-dir "./downloads"
```

### 3) Upload (TikTok - prototype flow)

```bash
dotnet run --project ClipFlow.Console -- upload-clip "./downloads/sample.mp4" --upload-service "tiktok"
```

Available `--upload-service` values:
- `TikTok`

## Architecture Overview

- `ClipFlow.Console` - CLI argument parsing and command routing.
- `ClipFlow.Application` - use cases, validation, and contracts (`IClipDownloader`, `IClipUploader`, `IClipUploaderResolver`).
- `ClipFlow.Infrastructure` - technical integrations (`yt-dlp`, Playwright, uploader resolver).
- `ClipFlow.Domain` - domain model layer (currently minimal).
- `ClipFlow.Tests` - unit tests for use cases and validators.

## Adding a New Upload Provider

1. Add a new value to `UploadServiceType`.
2. Implement `IClipUploader` in `ClipFlow.Infrastructure/Upload`.
3. Register the uploader in `ClipFlow.Infrastructure/DependencyInjection/DependencyInjection.cs`.
4. The resolver will map and select the correct uploader by `ServiceType`.

## Troubleshooting

### Playwright: "Executable doesn't exist"

Install Chromium binaries:

```bash
cd ./ClipFlow.Infrastructure/bin/Debug/net10.0/.playwright/package
node ./cli.js install chromium
```

### `yt-dlp` not found or failing

- Verify `yt-dlp` is installed and available in `PATH`.
- Check manually:

```bash
yt-dlp --version
```

## Next Steps

- Complete end-to-end TikTok publishing flow (login + file selection + publish).
- Add persistent browser session storage to avoid logging in on every run.
- Add more upload providers (for example YouTube Shorts, Instagram Reels).
