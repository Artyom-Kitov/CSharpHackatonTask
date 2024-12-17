using HackatonTaskLib.Harmony;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Task6RabbitMq.Config;
using Task6RabbitMq.Database;
using Task6RabbitMq.Messages;
using Task6RabbitMq.Services;

namespace Task6RabbitMq.AppTypes
{
    public class HrDirectorAppType : IAppType
    {
        public void Start(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConsole();
            builder.WebHost.UseUrls("http://*:5000");

            builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("Rabbit"));
            var rabbitMqConfig = builder.Configuration.GetSection("Rabbit").Get<RabbitMqConfig>()!;

            builder.Services
                .AddDbContext<SqliteDatabaseContext>(options => options.UseSqlite("Data Source=data/hackatondb.db"))
                .AddTransient<IHarmonyLevelCalculator, AverageHarmonicCalculator>()
                .AddSingleton<IHrDirectorService, HrDirectorService>()
                .AddMassTransit(x =>
                {
                    x.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host(rabbitMqConfig.Host, 5672, "/", h =>
                        {
                            h.Username(rabbitMqConfig.Username);
                            h.Password(rabbitMqConfig.Password);
                        });

                        cfg.Publish<HackatonStartedMessage>(x =>
                        {
                            x.ExchangeType = "fanout";
                        });
                    });
                })
                .AddHostedService<HrDirectorStartupService>()
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
