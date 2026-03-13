using ClipFlow.Application.UseCases.Download;

namespace ClipFlow.Tests.ApplicationTests.DownloadTests;

public class DownloadClipRequestValidatorTests
{
    private DownloadClipRequestValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DownloadClipRequestValidator();
    }

    [Test]
    public void Validate_Should_ReturnValid_WhenRequestIsCorrect()
    {
        var request = new DownloadClipRequest (
            Url: "https://www.twitch.tv/videos/123456789",
            OutputDirectory: "./downloads"
        );
        
        var result = _validator.Validate(request, out var uri);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(uri, Is.Not.Null);
        }
    }
}