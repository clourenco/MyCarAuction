namespace MyCarAuction.Api.Features.Users.Common.Exceptions
{
    public sealed class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("The specified user was not found.")
        {
        }

        public UserNotFoundException(string? message) : base(message)
        {
        }

        public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
