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
using MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;
using MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle;
using MyCarAuction.Api.Features.Auctions.Commands.StartAuction;
using MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;
using MyCarAuction.Api.Features.Auctions.Queries.GetAuction;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Bids.Repository;
using MyCarAuction.Api.Features.Users.Repository;
using MyCarAuction.Api.Features.Users.Service;
using MyCarAuction.Api.Infrastructure.Services;
using MyCarAuction.Api.Features.Bids.Commands.PlaceBid;
using System;
using MyCarAuction.Api.Infrastructure.Data;

namespace MyCarAuctionAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
            builder.Services.AddScoped<IBidRepository, BidRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IAuctionService, AuctionService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApiDbContext>>();
                await Seed.SeedData(contextFactory);
            }

            // Minimal api Vehicles:
            app.MapPost("/api/vehicles", async (CreateVehicleCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/vehicles/{result.Id}", result);
            });

            app.MapGet("/api/vehicle/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetVehicleQuery(id));
                return Results.Ok(result);
            });

            app.MapGet("/api/vehicles", async (string? type, string? manufacturer, string? model, int? year, IMediator mediator, CancellationToken cancellation) =>
            {
                var results = await mediator.Send(new SearchVehicleQuery(type, manufacturer, model, year));
                return Results.Ok(results);
            });


            // Minimal api Auctions:
            app.MapPost("/api/auctions", async (StartAuctionCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/auctions/{result.Id}", result);
            });

            app.MapPut("/api/auction/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new CloseAuctionCommand(id));
                return Results.Ok(result);
            });

            app.MapGet("/api/auction/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAuctionQuery(id));
                return Results.Ok(result);
            });

            // Minimal api Bids:
            app.MapPost("/api/bids", async (PlaceBidCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/bids/{result.Id}", result);
            });

            // Minimal api Users:


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
