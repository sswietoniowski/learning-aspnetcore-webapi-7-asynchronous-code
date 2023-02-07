using Covers.Api.Dtos;

namespace Covers.Api.Services;

public class FakeCoversService : ICoversService
{
    public async Task<CoverDto> GetCoverAsync(string coverId)
    {
        // generate a "cover" (byte array) between 5 and 10MB

        var random = new Random();

        const int MIN_COVER_SIZE = 5 * 1024 * 1024;
        const int MAX_COVER_SIZE = 10 * 1024 * 1024;

        int fakeContentBytes = random.Next(MIN_COVER_SIZE, MAX_COVER_SIZE);
        byte[] fakeContent = new byte[fakeContentBytes];

        random.NextBytes(fakeContent);

        return await Task.FromResult(new CoverDto(Id: coverId, Content: fakeContent));
    }
}