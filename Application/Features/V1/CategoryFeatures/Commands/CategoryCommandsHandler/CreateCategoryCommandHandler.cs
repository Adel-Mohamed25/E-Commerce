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
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<CreateCategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response<CreateCategoryModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null || httpContext.User.Identity?.IsAuthenticated == false)
                    return ResponseHandler.Unauthorized<CreateCategoryModel>(message: "Unauthorized request");

                var category = _mapper.Map<Category>(request.CreateCategoryModel);
                await _unitOfWork.Categories.CreateAsync(category, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return ResponseHandler.Success(data: request.CreateCategoryModel);
            }
            catch (Exception)
            {
                return ResponseHandler.Conflict(data: request.CreateCategoryModel);
            }

        }
    }
}
