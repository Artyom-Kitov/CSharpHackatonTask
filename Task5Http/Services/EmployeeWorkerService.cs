using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;
using Task5Http.Commons;

namespace Task5Http.Services
{
    public class EmployeeWorkerService(ILogger<EmployeeWorkerService> logger, IConfiguration configuration,
        EmployeeInfoHolder info, IHttpClientFactory clientFactory, IEmployeeWishlistGenerator generator,
        IHostApplicationLifetime lifetime) : BackgroundService
    {
        private readonly ILogger<EmployeeWorkerService> _logger = logger;
        private readonly HttpClient _client = clientFactory.CreateClient();
        private readonly IHostApplicationLifetime _lifetime = lifetime;

        private readonly string _hrManagerHost = configuration["Employee:HrManagerHost"]!;
        private readonly string _postUrl = info.Type == AppType.Junior
            ? configuration["Employee:HrManagerHost"]! + configuration["Employee:JuniorPath"]!
            : configuration["Employee:HrManagerHost"]! + configuration["Employee:TeamleadPath"]!;

        private readonly int _connectionTimeoutSeconds = int.Parse(configuration["Employee:TimeoutSeconds"]!);

        private readonly AppType _type = info.Type;
        private readonly int _id = info.Id;
        private readonly string _name = info.Name;
        private readonly Wishlist _wishlist = generator.Generate(info.Id,
            [.. (info.Type == AppType.Junior ? info.JuniorIds : info.TeamleadIds)]);

        public override async Task StartAsync(CancellationToken token)
        {
            await base.StartAsync(token);
            _logger.LogInformation("Starting {Type} with id = {Id} and name = {Name}", _type.Type, _id, _name);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            var client = new TimeoutPostHttpClient(_client, _logger, _postUrl, 1000 * _connectionTimeoutSeconds);
            await client.Send(_wishlist);

            _logger.LogInformation("Successfully sent wishlist to {Host}", _hrManagerHost);
            _lifetime.StopApplication();
        }
    }
}
