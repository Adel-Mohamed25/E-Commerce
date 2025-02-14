using Application.Features.CategoryFeatures.Commands.CategoryCommands;
using Application.Features.CategoryFeatures.Queries.CategoryQueries;
using Microsoft.AspNetCore.Mvc;
using Models.Category;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoryController : AppBaseController
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
        public async Task<IActionResult> Post([FromBody] PostCategoryModel postCategoryModel)
        {
            return NewResult(await Mediator.Send(new PostCategoryCommand(postCategoryModel)));
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put([FromQuery] string id, [FromBody] CategoryModel categoryModel)
        {
            return NewResult(await Mediator.Send(new PutCategoryCommand(id, categoryModel)));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }

    }
}
