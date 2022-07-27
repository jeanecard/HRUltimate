using CompanyEmployees.Presentation.ActionFilters;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        public AuthenticationController(IServiceManager service) => _service = service;
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await
            _service.AuthenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            var validationTask = await _service.AuthenticationService.ValidateUser(user);
            if (!validationTask.Item1)
                return Unauthorized();
            var tokenTask = await _service.AuthenticationService.CreateToken(true, validationTask.Item2);
            return Ok(tokenTask);
        }

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _service.AuthenticationService.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");
            var jwtFromGoogle = await _service.AuthenticationService.GetTokenForGoogle(externalAuth, payload);
            if (jwtFromGoogle == null)
            {
                return BadRequest("Invalid External Authentication.");
            }
            return Ok(jwtFromGoogle.Token);
        }
    }
}
