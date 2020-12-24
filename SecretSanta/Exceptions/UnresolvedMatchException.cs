using System;
using System.Runtime.Serialization;

namespace SecretSanta.Exceptions
{
    [Serializable]
    public class UnresolvedMatchException : Exception

    {
        public UnresolvedMatchException()
        {
        }

        protected UnresolvedMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnresolvedMatchException(string? message) : base(message)
        {
        }

        public UnresolvedMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}