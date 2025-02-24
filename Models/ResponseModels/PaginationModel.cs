using Domain.Enums;

namespace Models.ResponseModels
{
    public class PaginationModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public OrderBy OrderBy { get; set; }
        public OrderByDirection OrderByDirection { get; set; }

    }
}
