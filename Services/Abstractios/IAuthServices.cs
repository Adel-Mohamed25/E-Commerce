using Domain.Entities.Identity;
using Models.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Abstractions
{
    public interface IAuthServices
    {
        Func<string, JwtSecurityToken, Task<bool>> IsTokenValid { get; }
        Task<AuthModel> GetTokenAsync(User user);
        Task<JwtSecurityToken> ReadTokenAsync(string jwt);
        Task<AuthModel> GetRefreshTokenAsync(User user);
    }
}
