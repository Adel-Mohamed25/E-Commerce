using Domain.Entities.Identity;
using Google.Apis.Auth;
using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Authentication;
using Services.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementations
{
    public class AuthenticationService : IAuthServices
    {
        private readonly JWTSettings _jWTSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GoogleSettings _googleSettings;

        public AuthenticationService(IUnitOfWork unitOfWork, IOptions<JWTSettings> jWTSettings, IOptions<GoogleSettings> googleSettings)
        {
            _jWTSettings = jWTSettings.Value;
            _unitOfWork = unitOfWork;
            _googleSettings = googleSettings.Value;
        }


        Task<bool> IsTokenParametersValidAsync(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jWTSettings.ValidateIssuer,
                ValidIssuers = new[] { _jWTSettings.Issuer },
                ValidateIssuerSigningKey = _jWTSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = key,
                ValidAudience = _jWTSettings.Audience,
                ValidateAudience = _jWTSettings.ValidateAudience,
                ValidateLifetime = false,
            };
            try
            {
                var validator = handler.ValidateToken(jwt, parameters, out SecurityToken validatedToken);
                return Task.FromResult(validatedToken != null);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        Task<bool> IsTokenAlgorithmValidAsync(JwtSecurityToken jwtSecurityToken) =>
            Task.FromResult(jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature));

        public Func<string, JwtSecurityToken, Task<bool>> IsTokenValidAsync
         => async (jwt, jwtSecurityToken) =>
         await IsTokenParametersValidAsync(jwt) && await IsTokenAlgorithmValidAsync(jwtSecurityToken);


        public async Task<AuthModel> GetTokenAsync(User user)
        {
            var authModel = new AuthModel();

            if (user.JwtTokens.Any(jwt => jwt.IsRefreshTokenActive))
            {
                var activeUserToken = user.JwtTokens.Where(jwt => jwt.IsRefreshTokenActive).FirstOrDefault();

                authModel.TokenModel = new()
                {
                    Token = activeUserToken.Token,
                    TokenExpirationDate = activeUserToken.TokenExpirationDate,

                };

                authModel.RefreshTokenModel = new()
                {
                    RefreshToken = activeUserToken.RefreshToken,
                    RefreshTokenExpirationDate = activeUserToken.RefreshTokenExpirationDate
                };
            }
            else
            {
                var token = await GenerateTokenAsync(user, GetClaimsAsync);

                var refreshToken = GenerateRefreshToken();

                var JwtToken = new JwtToken
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpirationDate = DateTime.UtcNow.AddHours(_jWTSettings.AccessTokenExpireDate),
                    RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpireDate),
                    IsRefreshTokenUsed = true,
                    ModifiedDate = DateTime.Now,
                };

                using var trasaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.JwtTokens.CreateAsync(JwtToken);
                    await _unitOfWork.SaveChangesAsync();
                    await trasaction.CommitAsync();

                }
                catch
                {
                    await trasaction.RollbackAsync();
                }

                authModel.TokenModel = new()
                {
                    Token = JwtToken.Token,
                    TokenExpirationDate = JwtToken.TokenExpirationDate
                };

                authModel.RefreshTokenModel = new()
                {
                    RefreshToken = JwtToken.RefreshToken,
                    RefreshTokenExpirationDate = JwtToken.RefreshTokenExpirationDate
                };
            }
            return authModel;
        }

        private string GenerateRefreshToken()
        {
            //var newRefreshToken = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())));
            var randomNumber = new byte[64];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateTokenAsync(User user, Func<User, Task<List<Claim>>> Claims)
        {
            var claims = await Claims.Invoke(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtToken = new JwtSecurityToken(

                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jWTSettings.AccessTokenExpireDate),
                signingCredentials: signingCredentials
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }


        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var userRolesNames = await _unitOfWork.Users.UserManager.GetRolesAsync(user);
            var userClaims = await _unitOfWork.Users.UserManager.GetClaimsAsync(user);

            var userRoles = (await _unitOfWork.Roles.GetAllAsync(r => userRolesNames.Contains(r.Name ?? "Not Found"))).ToList();

            var permissions = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleClams = await _unitOfWork.Roles.RoleManager.GetClaimsAsync(role);
                permissions.AddRange(roleClams);
            }


            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new (ClaimTypes.PrimarySid, user.Id?.ToString() ?? throw new ArgumentNullException(nameof(user.Id))),
                new (ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new (ClaimTypes.Surname, user.LastName ?? string.Empty),
                new (ClaimTypes.Name, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                new (ClaimTypes.Email, user.Email ?? throw new ArgumentNullException(nameof(user.Email))),
            };


            foreach (var role in userRolesNames)
                claims.Add(new(ClaimTypes.Role, role));


            claims.AddRange(userClaims);
            claims.AddRange(permissions);

            return claims;
        }

        public Task<JwtSecurityToken> ReadTokenAsync(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            if (string.IsNullOrEmpty(jwt) || string.IsNullOrWhiteSpace(jwt))
                return Task.FromResult<JwtSecurityToken>(null);

            return Task.FromResult(handler.ReadJwtToken(jwt));
        }

        public async Task<AuthModel> GetRefreshTokenAsync(User user)
        {
            var jwtToken = user.JwtTokens.FirstOrDefault(u => u.IsRefreshTokenActive);
            if (jwtToken is null)
                return null;

            jwtToken.RefreshTokenRevokedDate = DateTime.UtcNow;

            var newToken = await GenerateTokenAsync(user, GetClaimsAsync);
            var newRefreshToken = GenerateRefreshToken();

            var newUserToken = new JwtToken
            {
                Id = jwtToken.Id,
                UserId = user.Id,
                Token = newToken,
                TokenExpirationDate = DateTime.UtcNow.AddHours(_jWTSettings.AccessTokenExpireDate),
                RefreshToken = newRefreshToken,
                RefreshTokenExpirationDate = jwtToken.RefreshTokenExpirationDate,
                IsRefreshTokenUsed = true
            };

            await _unitOfWork.JwtTokens.UpdateAsync(newUserToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthModel
            {
                TokenModel = new()
                {
                    Token = newUserToken.Token,
                    TokenExpirationDate = newUserToken.TokenExpirationDate
                },
                RefreshTokenModel = new()
                {
                    RefreshToken = newUserToken.RefreshToken,
                    RefreshTokenExpirationDate = newUserToken.RefreshTokenExpirationDate
                }
            };
        }


        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string ProviderKey)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(ProviderKey, settings);
            return payload;
        }

        public async Task<string> GenerateVerificationCodeAsync(User user)
        {
            var code = await _unitOfWork.Users.UserManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            return code;
        }

        public async Task<bool> VerifyCodeAsync(User user, string code)
        {
            var isValid = await _unitOfWork.Users.UserManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
            return isValid;
        }


    }
}
