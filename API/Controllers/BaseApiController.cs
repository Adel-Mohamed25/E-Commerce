using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.ResponseModels;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public ObjectResult NewResult<TData>(Response<TData> response) where TData : class
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK => new OkObjectResult(response),
                HttpStatusCode.NotFound => new NotFoundObjectResult(response),
                HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
                HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
                HttpStatusCode.Conflict => new ConflictObjectResult(response),
                _ => new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                }
            };
        }
    }
}
