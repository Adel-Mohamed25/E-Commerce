using Domain.Entities.Identity;
using Models.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Abstractions
{
    public interface IAuthServices
    {
        Func<string, JwtSecurityToken, Task<bool>> IsTokenValidAsync { get; }

        Task<AuthModel> GetTokenAsync(User user);

        Task<JwtSecurityToken> ReadTokenAsync(string jwt);

        Task<AuthModel> GetRefreshTokenAsync(User user);

        Task<string> GenerateVerificationCodeAsync(User user);

        Task<bool> VerifyCodeAsync(User user, string code);
    }
}
