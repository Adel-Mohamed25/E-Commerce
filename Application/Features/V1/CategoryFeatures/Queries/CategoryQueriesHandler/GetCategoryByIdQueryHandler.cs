using Application.Features.V1.CategoryFeatures.Queries.CategoryQueries;
using Application.Helper.ResponseServices;
using AutoMapper;
using Infrastructure.UnitOfWorks;
using MediatR;
using Models.Category;
using Models.ResponseModels;

namespace Application.Features.V1.CategoryFeatures.Queries.CategoryQueriesHandler
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Response<CategoryModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Categories.IsExistAsync(c => c.Id == request.Id, cancellationToken: cancellationToken))
            {
                return ResponseHandler.NotFound<CategoryModel>();
            }

            var category = await _unitOfWork.Categories.GetByAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);
            var data = _mapper.Map<CategoryModel>(category);
            return ResponseHandler.Success(data);
        }
    }
}
