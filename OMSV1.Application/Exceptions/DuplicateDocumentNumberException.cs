namespace OMSV1.Application.Exceptions
{
    public class DuplicateDocumentNumberException : Exception
    {
        public DuplicateDocumentNumberException(string documentNumber)
            : base($"A document with number '{documentNumber}' already exists.")
        {
        }
    }
}
