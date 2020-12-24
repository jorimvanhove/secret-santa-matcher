using System;
using System.Runtime.Serialization;

namespace SecretSanta.Exceptions
{
    [Serializable]
    public class InvalidMatchException : Exception
    {
        public InvalidMatchException()
        {
        }

        protected InvalidMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidMatchException(string? message) : base(message)
        {
        }

        public InvalidMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}