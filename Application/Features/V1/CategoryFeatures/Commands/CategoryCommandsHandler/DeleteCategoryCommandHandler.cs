﻿using Application.Features.V1.CategoryFeatures.Commands.CategoryCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Commands.CategoryCommandsHandler
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<CategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<CategoryModel>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User.Identity?.IsAuthenticated == false)
                return ResponseHandler.Unauthorized<CategoryModel>(message: "Unauthorized request");

            var category = await _unitOfWork.Categories.GetByAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
            var data = _mapper.Map<CategoryModel>(category);
            if (category is null)
            {
                return ResponseHandler.NotFound(data: data);
            }
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ResponseHandler.Success(data: data);

        }
    }
}
