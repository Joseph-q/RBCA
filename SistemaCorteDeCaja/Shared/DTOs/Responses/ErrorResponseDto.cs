namespace SistemaCorteDeCaja.Shared.DTOs.Responses
{
    public interface IErrorResponse : IResponse
    {
        string? Detail { get; set; }
        List<Dictionary<string, List<string>>>? Errors { get; set; }
        string? TraceId { get; set; }
    }

    public class ErrorResponseDto : IErrorResponse
    {
        public required string Title { get; set; }
        public string? Detail { get; set; }
        public List<Dictionary<string, List<string>>>? Errors { get; set; }
        public string? TraceId { get; set; }

    }
}
