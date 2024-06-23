using System.Runtime.Serialization;

namespace ZigZag.Domain.Exceptions
{
    public class InsufficientAccessException : Exception
    {
        public InsufficientAccessException() { }

        public InsufficientAccessException(string message)
            : base(message) { }

        public InsufficientAccessException(string message, Exception innerException)
            : base(message, innerException) { }

        protected InsufficientAccessException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
