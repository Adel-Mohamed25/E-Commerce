using Domain.Entities.Identity;
using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Authentication;
using Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Services
{
    public class AuthenticationService : IAuthenticationServices
    {
        private readonly JWTSettings _jWTSettings;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService(IUnitOfWork unitOfWork, IOptions<JWTSettings> options)
        {
            _jWTSettings = options.Value;
            _unitOfWork = unitOfWork;
        }

        async Task<bool> IsJWTAlgorithmValidAsync(JwtSecurityToken jwtSecurityToken)
        {
            return await Task.FromResult(jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature));
        }
        Task<bool> IsJWTParametersValidAsync(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jWTSettings.ValidateIssuer,
                ValidIssuers = new[] { _jWTSettings.Issuer },
                ValidateIssuerSigningKey = _jWTSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jWTSettings.Secret)),
                ValidAudience = _jWTSettings.Audience,
                ValidateAudience = _jWTSettings.ValidateAudience,
                ValidateLifetime = _jWTSettings.ValidateLifeTime,
            };
            try
            {
                var validator = handler.ValidateToken(jwt, parameters, out SecurityToken validatedToken);

                if (validatedToken == null)
                    return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        public Func<string, JwtSecurityToken, Task<bool>> IsTokenValid => async (jwt, jwtSecurityToken) => await IsJWTParametersValidAsync;

        public Task<AuthModel> GetTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<JwtSecurityToken> ReadTokenAsync(string jwt)
        {
            throw new NotImplementedException();
        }

        public Task<AuthModel> RefreshTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
