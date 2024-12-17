using HackatonTaskLib.WishlistGeneration;
using MassTransit;
using Task6RabbitMq.Config;
using Task6RabbitMq.Services;

namespace Task6RabbitMq.AppTypes
{
    public enum EmployeeType
    {
        Junior,
        Teamlead
    }

    public class EmployeeAppType(EmployeeType employeeType) : IAppType
    {
        public void Start(string[] args)
        {
            int id = int.Parse(args[1]);

            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConsole();

            builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("Rabbit"));
            var rabbitMqConfig = builder.Configuration.GetSection("Rabbit").Get<RabbitMqConfig>()!;

            builder.Services
                .AddSingleton(provider => new EmployeeInfoHolder(employeeType, id))
                .AddTransient<EmployeeConsumer>()
                .AddTransient<IEmployeeWishlistGenerator, EmployeeRandomWishlistGenerator>()
                .AddMassTransit(x =>
                {
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(rabbitMqConfig.Host, "/", h =>
                        {
                            h.Username(rabbitMqConfig.Username);
                            h.Password(rabbitMqConfig.Password);
                        });

                        string prefix = employeeType == EmployeeType.Junior ? rabbitMqConfig.JuniorPrefix : rabbitMqConfig.TeamleadPrefix;
                        string queue = prefix + id;
                        cfg.ReceiveEndpoint(queue, e =>
                        {
                            e.Consumer<EmployeeConsumer>(ctx);
                        });
                    });
                });

            var app = builder.Build();
            app.Run();
        }
    }
}
