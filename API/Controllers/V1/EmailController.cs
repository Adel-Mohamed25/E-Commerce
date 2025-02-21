using Application.Features.V1.EmailFeatures.Commands.EmailCommands;
using Microsoft.AspNetCore.Mvc;
using Models.Email;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmailController : AppBaseController
    {
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailModel sendEmail)
        {
            return NewResult(await Mediator.Send(new SendEmailCommand(sendEmail)));
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailRequest emailRequest)
        {
            return NewResult(await Mediator.Send(new ConfirmEmailCommand(emailRequest)));
        }
    }
}
