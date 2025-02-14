using Domain.Entities.Identity;
using Infrastructure.Settings;
using Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Authentication;
using Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public Func<string, JwtSecurityToken, Task<bool>> IsTokenValid
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
                var accessToken = await GenerateTokenAsync(user, GetClaimsAsync);

                var refreshTokenModel = GetRefreshToken();

                var JwtToken = new JwtToken
                {
                    Token = accessToken,
                    RefreshToken = refreshTokenModel.RefreshToken,
                    IsRefreshTokenUsed = true,
                    UserId = user.Id,
                    TokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.AccessTokenExpireDate),
                    RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpireDate),
                };

                using var trasaction = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    await _unitOfWork.JwtTokens.CreateAsync(JwtToken);
                    //await _unitOfWork.Users.UpdateAsync(user);
                    //var identityResult = await _unitOfWork.Users.Manager.UpdateAsync(user);

                    await _unitOfWork.SaveChangesAsync();
                    await trasaction.CommitAsync();

                    //if (!identityResult.Succeeded)
                    //    return new AuthenticationModel();
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

        private RefreshTokenModel GetRefreshToken()
        {
            var refreshToken = new RefreshTokenModel
            {
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpireDate),
            };
            return refreshToken;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateTokenAsync(User user, Func<User, Task<List<Claim>>> getClaims)
        {
            var claims = await getClaims.Invoke(user);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jWTSettings.Secret));
            var jwtToken =
                  new JwtSecurityToken(
                  issuer: _jWTSettings.Issuer,
                  audience: _jWTSettings.Audience,
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(_jWTSettings.AccessTokenExpireDate),
                  signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature));
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }

        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var userRolesNames = await _unitOfWork.Users.UserManager.GetRolesAsync(user);
            var userClaims = await _unitOfWork.Users.UserManager.GetClaimsAsync(user);

            #region Get Permissions

            // get user roles
            var userRoles = (await _unitOfWork.Roles.GetAllAsync(r => userRolesNames.Contains(r.Name))).ToList();
            // get role claims
            var permissions = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleClams = await _unitOfWork.Roles.RoleManager.GetClaimsAsync(role);
                permissions.AddRange(roleClams);
            }

            #endregion

            var claims = new List<Claim>()
            {
                new (ClaimTypes.PrimarySid, user.Id),
                new (ClaimTypes.Name,user.UserName),
                new (ClaimTypes.Email,user.Email),
                new (ClaimTypes.MobilePhone, user.PhoneNumber),
            };


            foreach (var role in userRolesNames)
                claims.Add(new(ClaimTypes.Role, role));


            claims.AddRange(userClaims);
            claims.AddRange(permissions);

            return claims;
        }

        public Task<JwtSecurityToken> ReadTokenAsync(string jwt)
        {
            if (string.IsNullOrEmpty(jwt) || string.IsNullOrWhiteSpace(jwt))
                return null;

            return Task.FromResult(new JwtSecurityTokenHandler().ReadJwtToken(jwt));
        }

        Task<bool> IsTokenParametersValidAsync(string jwt)
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

        async Task<bool> IsTokenAlgorithmValidAsync(JwtSecurityToken jwtSecurityToken) =>
            await Task.FromResult(jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature));

        public async Task<AuthModel> RefreshTokenAsync(User user)
        {
            var JwtToken = user.JwtTokens.FirstOrDefault(u => u.IsRefreshTokenActive);

            if (JwtToken is null)
                return null;

            // revoke refresh Token
            JwtToken.RefreshTokenRevokedDate = DateTime.UtcNow;

            var jwt = await GenerateTokenAsync(user, GetClaimsAsync);
            var refreshToken = GenerateRefreshToken();

            // add new refresh token to user
            var newUserToken = new JwtToken()
            {
                UserId = user.Id,
                Token = jwt,
                TokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.AccessTokenExpireDate),
                RefreshToken = refreshToken,
                RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpireDate),
                IsRefreshTokenUsed = true,
            };
            user.JwtTokens.Add(newUserToken);

            var identityResult = await _unitOfWork.Users.UserManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
                return null;

            return new AuthModel()
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

    }
}
