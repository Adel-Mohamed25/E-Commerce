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
    public class PutCategoryCommandHandler : IRequestHandler<PutCategoryCommand, Response<CategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PutCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<CategoryModel>> Handle(PutCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                    return ResponseHandler.Success<CategoryModel>(data: request.CategoryModel);
                }
            }
            catch (Exception)
            {
                return ResponseHandler.Conflict<CategoryModel>(data: request.CategoryModel);
            }

        }
    }
}
