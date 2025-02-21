using Application.Features.V1.CategoryFeatures.Commands.CategoryCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Commands.CategoryCommandsHandler
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<CreateCategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<CreateCategoryModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = _mapper.Map<Category>(request.PostCategoryModel);
                await _unitOfWork.Categories.CreateAsync(data, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return ResponseHandler.Success(data: request.PostCategoryModel);
            }
            catch (Exception)
            {
                return ResponseHandler.Conflict(data: request.PostCategoryModel);
            }

        }
    }
}
