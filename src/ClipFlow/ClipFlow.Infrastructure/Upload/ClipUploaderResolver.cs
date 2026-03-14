using ClipFlow.Application.Abstractions.Upload;
using ClipFlow.Application.Common;

namespace ClipFlow.Infrastructure.Upload;

public sealed class ClipUploaderResolver(IEnumerable<IClipUploader> clipUploaders) : IClipUploaderResolver
{
    private readonly IReadOnlyDictionary<UploadServiceType, IClipUploader> _uploaders = BuildUploaderMap(clipUploaders);

    public Result<IClipUploader> Resolve(UploadServiceType serviceType)
    {
        if (_uploaders.TryGetValue(serviceType, out var uploader))
            return Result<IClipUploader>.Success(uploader);

        var supportedServices = string.Join(", ", _uploaders.Keys.OrderBy(x => x).Select(x => x.ToString()));

        return Result<IClipUploader>.Failure(
            ErrorType.NotFound,
            $"Uploader for service '{serviceType}' is not registered. Supported services: {supportedServices}");
    }

    private static IReadOnlyDictionary<UploadServiceType, IClipUploader> BuildUploaderMap(IEnumerable<IClipUploader> clipUploaders)
    {
        var uploaderMap = new Dictionary<UploadServiceType, IClipUploader>();

        foreach (var uploader in clipUploaders)
        {
            if (!uploaderMap.TryAdd(uploader.ServiceType, uploader))
            {
                throw new InvalidOperationException(
                    $"Multiple uploaders registered for service type '{uploader.ServiceType}'.");
            }
        }

        return uploaderMap;
    }
}

