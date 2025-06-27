using API.Entities;

namespace API.ServiceContracts;

public interface ITokenService
{
    public Task<string> CreateToken(AppUser user);
}
