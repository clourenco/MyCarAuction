using MediatR;

namespace MyCarAuction.Api.Features.Vehicles.Queries.GetVehicleById
{
    public sealed record GetVehicleByIdQuery(Guid Id) : IRequest<GetVehicleByIdResponse>;
}
