using Application.Features.V1.CategoryFeatures.Commands.CategoryCommands;
using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Category;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.V1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        [HttpGet("GetAll")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll()
        {
            return NewResult(await Mediator.Send(new GetAllCategoriesQuery()));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            return NewResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] CreateCategoryModel createCategoryModel)
        {
            return NewResult(await Mediator.Send(new CreateCategoryCommand(createCategoryModel)));
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put([FromQuery] string id, [FromBody] CategoryModel categoryModel)
        {
            return NewResult(await Mediator.Send(new UpdateCategoryCommand(id, categoryModel)));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }

    }
}
