using Application.Features.CategoryFeatures.Commands.CategoryCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Commands.CategoryCommandsHandler
{
    public class PostCategoryCommandHandler : IRequestHandler<PostCategoryCommand, Response<PostCategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<PostCategoryModel>> Handle(PostCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = _mapper.Map<Category>(request.PostCategoryModel);
                await _unitOfWork.Categories.CreateAsync(data, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return ResponseHandler.Success<PostCategoryModel>(data: request.PostCategoryModel);
            }
            catch (Exception)
            {
                return ResponseHandler.Conflict<PostCategoryModel>(data: request.PostCategoryModel);
            }

        }
    }
}
