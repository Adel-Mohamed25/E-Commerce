using Application.Features.V1.RoleFeatures.Comands.RoleCommands;
using Application.Features.V1.RoleFeatures.Queries.RoleQueries;
using Microsoft.AspNetCore.Mvc;
using Models.Role;

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : BaseApiController
    {
        [HttpGet("GetAll")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll()
        {
            return NewResult(await Mediator.Send(new GetAllRolesQuery()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            return NewResult(await Mediator.Send(new GetRoleByIdQuery(id)));
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] CreateRoleModel createRoleModel)
        {
            return NewResult(await Mediator.Send(new CreateRoleCommand(createRoleModel)));
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put([FromQuery] string id, [FromBody] RoleModel roleModel)
        {
            return NewResult(await Mediator.Send(new UpdateRoleCommand(id, roleModel)));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            return NewResult(await Mediator.Send(new DeleteRoleCommand(id)));
        }

    }
}
