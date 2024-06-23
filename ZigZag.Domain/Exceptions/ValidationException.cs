using System.Runtime.Serialization;

namespace ZigZag.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public string Title { get; set; }

        public ValidationException() { }

        public ValidationException(string message)
            : base(message) { }

        public ValidationException(string? title, string message)
            : base(message) { Title = title; }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
