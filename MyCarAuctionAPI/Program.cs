using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;
using MyCarAuctionAPI.Middleware;
using MyCarAuctionAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyCarAuctionAPI.Features.Vehicles.Repository;
using MyCarAuctionAPI.Features.Vehicles.Interfaces.Services;
using MyCarAuctionAPI.Features.Vehicles.Services;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MediatR;
using MyCarAuction.Api.PipelineBehavior;
using MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;
using MyCarAuction.Api.Features.Vehicles.Queries.GetVehicleById;
using MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle;

namespace MyCarAuctionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContextFactory<ApiDbContext>(options => options.UseInMemoryDatabase("MyCarAuctionDb"));

            builder.Services
                .AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));


            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            var app = builder.Build();

            // Minimal api:
            app.MapPost("/api/vehicles", async (CreateVehicleCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/vehicles/{result.Id}", result);
            });

            app.MapGet("/api/vehicle/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetVehicleByIdQuery(id));
                return Results.Ok(result);
            });

            app.MapGet("/api/vehicles", async (string? type, string? manufacturer, string? model, int? year, IMediator mediator, CancellationToken cancellation) =>
            {
                var results = await mediator.Send(new SearchVehicleQuery(type, manufacturer, model, year));
                return Results.Ok(results);
            });


            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
