
using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared;
using Shared.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private JwtConfiguration _jwtConfig;
        private GoogleConfiguration _googleConfig;

        public AuthenticationService(
            ILoggerManager logger, 
            IMapper mapper, 
            UserManager<User> userManager,
            IOptions<JwtConfiguration> configJwt,
            IOptions<GoogleConfiguration> configGoogle)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _jwtConfig = configJwt.Value;
            _googleConfig = configGoogle.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForAuth"></param>
        /// <returns></returns>
        public async Task<(bool, User)> ValidateUser(UserForAuthenticationDto userForAuth)
        {
            var user = await _userManager.FindByNameAsync(userForAuth.UserName);
            var result = (user != null && await _userManager.CheckPasswordAsync(user, userForAuth.Password));
            if (!result)
                _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");
            return (result, user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public async Task<string> CreateToken()
        public async Task<TokenDto> CreateToken(bool populateExp, User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            if (populateExp)
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7).ToUniversalTime();
            await _userManager.UpdateAsync(user);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenDto(accessToken, refreshToken);

            //return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Constants.SECRET_KEY) ?? String.Empty);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
             {
             new Claim(ClaimTypes.Name, user.UserName)
             };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken
            (
            issuer: _jwtConfig.ValidIssuer,
            audience: _jwtConfig.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfig.Expires)),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Constants.SECRET_KEY) ?? String.Empty)),
                ValidateLifetime = true,
                ValidIssuer = _jwtConfig.ValidIssuer,
                ValidAudience = _jwtConfig.ValidAudience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out
           securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
           !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequest();
//            _user = user;
            return await CreateToken(populateExp: false, user);
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _googleConfig.ClientId ?? String.Empty }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }

        public async Task<AuthResponseDto> GetTokenForGoogle(ExternalAuthDto extAuth, GoogleJsonWebSignature.Payload payload)
        {
            var info = new UserLoginInfo(extAuth.Provider, payload.Subject, extAuth.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email, FirstName = "Saucisse", LastName = "Saumon" };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, "Manager");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }
            //check for the Locked out account
            var token = await CreateToken(populateExp: true, user);
            return new AuthResponseDto { 
                Token = token.AccessToken, 
                IsAuthSuccessful = true, 
                RefreshToken = token.RefreshToken,
                Provider = extAuth.Provider
            };

        }
    }
}
