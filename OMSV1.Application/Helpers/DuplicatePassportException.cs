using System;

namespace OMSV1.Application.Helpers
{
    public class DuplicatePassportException : Exception
    {
        public DuplicatePassportException(string message) : base(message) { }
        public DuplicatePassportException(string message, Exception innerException) : base(message, innerException) { }
    }
}
