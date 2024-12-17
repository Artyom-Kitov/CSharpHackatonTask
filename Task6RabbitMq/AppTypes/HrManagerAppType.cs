using HackatonTaskLib.Strategy;
using MassTransit;
using Nsu.HackathonProblem.Contracts;
using Task6RabbitMq.Config;
using Task6RabbitMq.Messages;
using Task6RabbitMq.Services;

namespace Task6RabbitMq.AppTypes
{
    public class HrManagerAppType : IAppType
    {
        public void Start(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConsole();

            builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("Rabbit"));
            var rabbitMqConfig = builder.Configuration.GetSection("Rabbit").Get<RabbitMqConfig>()!;

            builder.Services
                .AddTransient<ITeamBuildingStrategy, GaleShapleyStrategy>()
                .AddTransient<HrManagerConsumer>()
                .AddSingleton<IHrManagerService, HrManagerService>()
                .AddHttpClient()
                .AddMassTransit(x =>
                {
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(rabbitMqConfig.Host, 5672, "/", h =>
                        {
                            h.Username(rabbitMqConfig.Username);
                            h.Password(rabbitMqConfig.Password);
                        });

                        cfg.Publish<WishlistMessage>(x =>
                        {
                            x.ExchangeType = "fanout";
                        });

                        cfg.ReceiveEndpoint(rabbitMqConfig.HrManagerQueue, e =>
                        {
                            e.Consumer<HrManagerConsumer>(ctx);
                        });
                    });
                })
                .AddControllers();
            
            var app = builder.Build();
            app.UseRouting();
            app.MapControllerRoute(
                name: "HrDirector",
                pattern: "api/{controller=HrDirector}/{action}");

            app.Run();
        }
    }
}
