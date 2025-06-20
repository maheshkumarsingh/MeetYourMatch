using API.Entities;

namespace API.ServiceContracts;

public interface ITokenService
{
    public string CreateToken(AppUser user);
}
