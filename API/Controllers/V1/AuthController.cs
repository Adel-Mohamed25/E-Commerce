using Application.Features.V1.UserFeatures.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;
using Models.Authentication;
using Models.User;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]

    public class AuthController : AppBaseController
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserModel postUserModel)
        {
            return NewResult(await Mediator.Send(new RegisterUserCommand(postUserModel)));
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

    }
}
