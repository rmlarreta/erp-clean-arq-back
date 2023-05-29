namespace Erp.Api.Application.Dtos.Commons
{
    public class DataResponse<T>
    {
        public T? Data { get; set; }
        public ErrorResponse? ErrorResponse { get; set; }

        public DataResponse(T data)
        {
            Data = data;
        }

        public DataResponse(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }
    }
}
