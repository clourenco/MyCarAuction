namespace MyCarAuction.Api.Common.Exceptions
{
    public sealed class KeyViolationException : Exception
    {
        public KeyViolationException() : base("The provided key already exists.")
        {
        }

        public KeyViolationException(string? message) : base(message)
        {
        }

        public KeyViolationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
