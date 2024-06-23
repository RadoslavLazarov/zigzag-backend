using System;
using System.Runtime.Serialization;

namespace ZigZag.Domain.Exceptions
{
    public class NotAcceptableException : Exception
    {
        public string Title { get; set; }

        public NotAcceptableException() { }

        public NotAcceptableException(string message)
            : base(message) { }

        public NotAcceptableException(string? title, string message)
            : base(message) { Title = title; }

        public NotAcceptableException(string message, Exception innerException)
            : base(message, innerException) { }

        protected NotAcceptableException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
