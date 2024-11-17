namespace MyCarAuction.Api.Common.CustomException;

internal sealed class VehicleSoldException : Exception
{
    public VehicleSoldException() : base("The specified vehicle was sold")
    {
    }

    public VehicleSoldException(string? message) : base(message)
    {
    }

    public VehicleSoldException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
