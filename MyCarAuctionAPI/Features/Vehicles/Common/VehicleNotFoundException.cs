namespace MyCarAuction.Api.Features.Vehicles.Common
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException() : base("The specified vehicle was not found.")
        {
        }

        public VehicleNotFoundException(string? message) : base(message)
        {
        }

        public VehicleNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
