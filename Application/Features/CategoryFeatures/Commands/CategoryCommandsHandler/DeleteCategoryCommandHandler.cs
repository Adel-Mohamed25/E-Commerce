using Application.Features.CategoryFeatures.Commands.CategoryCommands;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.CategoryFeatures.Commands.CategoryCommandsHandler
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<CategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CategoryModel>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.Categories.GetByAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
            var data = _mapper.Map<CategoryModel>(category);
            if (category is null)
            {
                return ResponseHandler.NotFound<CategoryModel>(data: data);
            }
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ResponseHandler.Success<CategoryModel>(data: data);

        }
    }
}
