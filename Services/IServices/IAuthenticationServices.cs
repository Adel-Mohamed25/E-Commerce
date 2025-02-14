using Domain.Entities.Identity;
using Models.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Services.IServices
{
    public interface IAuthenticationServices
    {
        Func<string, JwtSecurityToken, Task<bool>> IsTokenValid { get; }
        Task<AuthModel> GetTokenAsync(User user);
        Task<JwtSecurityToken> ReadTokenAsync(string jwt);
        Task<AuthModel> RefreshTokenAsync(User user);
    }
}
