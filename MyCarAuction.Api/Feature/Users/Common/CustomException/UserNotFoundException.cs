namespace MyCarAuction.Api.Features.Users.Common.CustomException
{
    internal sealed class UserNotFoundException : Exception
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
