using DatabaseEntitiesLib.Entity;
using MassTransit;
using Task6RabbitMq.Database;
using Task6RabbitMq.Messages;

namespace Task6RabbitMq.Services
{
    public class HrDirectorStartupService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000, stoppingToken);

            using var scope = _serviceScopeFactory.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            var dbContext = scope.ServiceProvider.GetRequiredService<SqliteDatabaseContext>();

            var hackaton = new Hackaton() { Harmony = 0.0, Teams = [] };
            dbContext.Hackatons.Add(hackaton);
            dbContext.SaveChanges();

            await publishEndpoint.Publish(new HackatonStartedMessage() { HackatonId = hackaton.Id }, stoppingToken);
        }
    }
}
