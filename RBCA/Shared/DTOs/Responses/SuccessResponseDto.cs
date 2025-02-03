namespace SistemaCorteDeCaja.Shared.DTOs.Responses
{
    public interface ISuccessResponse : IResponse
    {
        object? Data { get; set; }
    }

    public class SuccessResponseDto : ISuccessResponse
    {
        public string Title { get; set; } = "Success";
        public object? Data { get; set; }
    }
}
