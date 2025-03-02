using Application.Features.V1.CategoryFeatures.Commands.CategoryCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Commands.CategoryCommandsHandler
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<CategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<CategoryModel>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null || httpContext.User.Identity?.IsAuthenticated == false)
                    return ResponseHandler.Unauthorized<CategoryModel>(message: "Unauthorized request");

                if (request.Id != request.CategoryModel.Id)
                {
                    return ResponseHandler.BadRequest<CategoryModel>(errors: "Different identifier in both cases");
                }
                else
                {
                    var category = await _unitOfWork.Categories.GetByAsync(c => c.Id == request.Id, null, cancellationToken: cancellationToken);
                    if (category == null)
                        return ResponseHandler.NotFound<CategoryModel>();
                    else
                    {
                        var data = _mapper.Map<Category>(request.CategoryModel);
                        await _unitOfWork.Categories.UpdateAsync(data, cancellationToken);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    return ResponseHandler.Success(data: request.CategoryModel);
                }
            }
            catch (Exception)
            {
                return ResponseHandler.Conflict(data: request.CategoryModel);
            }

        }
    }
}
