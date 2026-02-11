namespace ClipFlow.Application.Common;

public enum ErrorType
{
    None = 0,
    Validation,
    NotFound,
    Conflict,
    ExternalService,
    Infrastructure,
    Unexpected
}