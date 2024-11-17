using FluentValidation;
using System.Reflection;
using FluentValidation.AspNetCore;
using MyCarAuction.Api.Middleware;
using MyCarAuction.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Features.Vehicles.Repository;
using MyCarAuction.Api.Features.Vehicles.Interfaces.Services;
using MyCarAuction.Api.Features.Vehicles.Services;
using MediatR;
using MyCarAuction.Api.Features.Vehicles.Commands.CreateVehicle;
using MyCarAuction.Api.Features.Vehicles.Queries.GetVehicle;
using MyCarAuction.Api.Features.Vehicles.Queries.SearchVehicle;
using MyCarAuction.Api.Features.Auctions.Commands.StartAuction;
using MyCarAuction.Api.Features.Auctions.Commands.CloseAuction;
using MyCarAuction.Api.Features.Auctions.Queries.GetAuction;
using MyCarAuction.Api.Features.Auctions.Repository;
using MyCarAuction.Api.Features.Users.Repository;
using MyCarAuction.Api.Features.Users.Service;
using MyCarAuction.Api.Infrastructure.Pipeline.Behavior;
using MyCarAuctionAPI.Features.Vehicles.Repository;
using MyCarAuction.Api.Features.Users.Commands.CreateUser;
using MyCarAuction.Api.Features.Users.Queries.GetUser;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using MyCarAuction.Api.Feature.Auctions.Command.PlaceBid;
using MyCarAuction.Api.Feature.Auctions.Repository;
using MyCarAuction.Api.Feature.Auctions.Service;

namespace MyCarAuction.Api
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
            IncludeVehicleMinimalApi(app);


            // Minimal api Auctions:
            IncludeAuctionMinimalApi(app);

            // Minimal api Bids:
            IncludeBidMinimalApi(app);

            // Minimal api Users:
            IncludeUserMinimalApi(app);

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

        #region Private methods

        private static void IncludeUserMinimalApi(WebApplication app)
        {
            /// <summary>
            /// Creates a user.
            /// </summary>
            app.MapPost("/api/users", async (CreateUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/users/{result.Id}", result);
            })
            .WithDescription("Adds a user");
            //.WithOpenApi(_ => new OpenApiOperation
            //{
            //    Summary = "Adds a user",
            //    Tags = [new() { Name = "MyCarAuction.Api" }]
            //});

            app.MapGet("/api/user/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetUserQuery(id));
                return Results.Ok(result);
            })
            .WithDescription("Gets a user");
        }

        private static void IncludeBidMinimalApi(WebApplication app)
        {
            app.MapPost("/api/bids", async (PlaceBidCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/bids/{result.Id}", result);
            })
            .WithDescription("Places a bid");
        }

        private static void IncludeAuctionMinimalApi(WebApplication app)
        {
            app.MapPost("/api/auctions", async (StartAuctionCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/auctions/{result.Id}", result);
            })
            .WithDescription("Starts an auction");

            app.MapPut("/api/auction/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new CloseAuctionCommand(id));
                return Results.Ok(result);
            })
            .WithDescription("Closes an auction");

            app.MapGet("/api/auction/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAuctionQuery(id));
                return Results.Ok(result);
            })
            .WithDescription("Gets an auction");
        }

        private static void IncludeVehicleMinimalApi(WebApplication app)
        {
            app.MapPost("/api/vehicles", async (CreateVehicleCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return Results.Created($"/api/vehicles/{result.Id}", result);
            })
            .WithDescription("Adds a vehicle");

            app.MapGet("/api/vehicle/{id}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetVehicleQuery(id));
                return Results.Ok(result);
            })
            .WithDescription("Gets a vehicle");

            app.MapGet("/api/vehicles", async (string? type, string? manufacturer, string? model, int? year, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var results = await mediator.Send(new SearchVehicleQuery(type, manufacturer, model, year), cancellationToken);
                return Results.Ok(results);
            })
            .WithDescription("Searches for a vehicle");
        }

        #endregion
    }
}
