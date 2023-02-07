namespace Covers.Api.Services;

using Covers.Api.Dtos;

public interface ICoversService
{
    Task<CoverDto> GetCoverAsync(string id);
}