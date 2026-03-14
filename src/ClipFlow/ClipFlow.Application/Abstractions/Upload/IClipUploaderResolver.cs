using ClipFlow.Application.Common;

namespace ClipFlow.Application.Abstractions.Upload;

public interface IClipUploaderResolver
{
    Result<IClipUploader> Resolve(UploadServiceType serviceType);
}

