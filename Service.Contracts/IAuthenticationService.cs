using Entities.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
        Task<(bool, User)> ValidateUser(UserForAuthenticationDto userForAuth);
        //Task<string> CreateToken();
        Task<TokenDto> CreateToken(bool populateExp, User user);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth);
        Task<AuthResponseDto> GetTokenForGoogle(ExternalAuthDto extAuth, GoogleJsonWebSignature.Payload payload);
    }
}
