using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;
using Models.Authentication;
using Models.User;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class AuthController : BaseApiController
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserModel createUserModel)
        {
            return NewResult(await Mediator.Send(new RegisterUserCommand(createUserModel)));
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            return NewResult(await Mediator.Send(new LoginUserCommand(loginModel)));
        }

        [HttpPost("GoogleExternalLogin")]
        public async Task<IActionResult> GoogleExternalLogin([FromBody] ExternalAuthRequest externalAuth)
        {
            return NewResult(await Mediator.Send(new GoogleExternalLoginCommand(externalAuth)));
        }

        [HttpPost("FacebookExternalLogin")]
        public async Task<IActionResult> FacebookExternalLogin([FromBody] ExternalAuthRequest externalAuth)
        {
            return NewResult(await Mediator.Send(new FacebookExternalLoginCommand(externalAuth)));
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel refreshTokenRequest)
        {
            return NewResult(await Mediator.Send(new RefreshTokenCommand(refreshTokenRequest)));
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyEmailModel email)
        {
            return NewResult(await Mediator.Send(new VerifyUserCommand(email)));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            return NewResult(await Mediator.Send(new ResetPasswordCommand(resetPassword)));
        }
    }
}
