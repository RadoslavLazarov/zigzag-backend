namespace ZigZag.Domain.Models
{
    public class ErrorModel
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public IEnumerable<ValidationErrorModel> Errors { get; set; }

        public string? ExceptionType { get; set; }

        public string? StackTrace { get; set; }
    }
}
