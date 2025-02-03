namespace SistemaCorteDeCaja.Shared.DTOs.Responses
{
    public interface ISuccessResponse<T> : IResponse
    {
        T Data { get; set; }
    }

    public class SuccessResponseDto<T> : ISuccessResponse<T>
    {
        public string Title { get; set; } = "Success";
        public T Data { get; set; } = default!;
    }

    public class SuccessResponseDto : SuccessResponseDto<object>
    {
    }
}
