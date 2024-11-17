using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MyCarAuction.Tests.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<MyCarAuction.Api.Program>
{
    public Mock<IMediator> MediatorMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IMediator));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton(MediatorMock.Object);
        });
    }
}
