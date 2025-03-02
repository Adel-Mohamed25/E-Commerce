using Application.Constants;
using Models.ResponseModels;
using System.Net;

namespace Application.Helper.ResponseServices
{
    public static class PaginationResponseHandler
    {
        public static PaginationResponse<TData> Success<TData>(
            TData data = null,
            string message = null,
            string meta = null,
            string errors = null,
            int totalCount = 0,
            int pageNumber = 1,
            int pageSize = 10) where TData : class
        {
            return new PaginationResponse<TData>(
                statusCode: HttpStatusCode.OK,
                issucceeded: true,
                message: message ?? ResponseMessage.SuccessMessage,
                meta: meta,
                data: data,
                errors: errors ?? "There are no errore",
                totalCount: totalCount,
                pageNumber: pageNumber,
                pageSize: pageSize

            );
        }

        public static PaginationResponse<TData> NotFound<TData>(
            string message = null,
            string meta = null,
            string errors = null,
            TData data = null,
            int totalCount = 0,
            int pageNumbre = 1,
            int pageSize = 10) where TData : class
        {
            return new PaginationResponse<TData>(
                statusCode: HttpStatusCode.NotFound,
                issucceeded: true,
                message: message ?? ResponseMessage.NotFoundMessage,
                meta: meta,
                data: data,
                errors: errors ?? "Not found data",
                totalCount: totalCount,
                pageNumber: pageNumbre,
                pageSize: pageSize

            );
        }

        public static PaginationResponse<TData> Unauthorized<TData>(
            string message = null,
            string meta = null,
            string errors = null,
            TData data = null,
            int totalCount = 0,
            int pageNumbre = 1,
            int pageSize = 10) where TData : class
        {
            return new PaginationResponse<TData>(
                statusCode: HttpStatusCode.Unauthorized,
                issucceeded: true,
                message: message ?? ResponseMessage.UnAuthorizedMessage,
                meta: meta,
                data: data,
                errors: errors ?? "Unauthorized request",
                totalCount: totalCount,
                pageNumber: pageNumbre,
                pageSize: pageSize

            );
        }
    }
}
