using Nsu.HackathonProblem.Contracts;
using Task6RabbitMq.Commons;
using Task6RabbitMq.Messages;
using Task6RabbitMq.Requests;

namespace Task6RabbitMq.Services
{
    public interface IHrManagerService
    {
        Task HandleWishlistMessage(WishlistMessage message);
    }

    public class HrManagerService(ILogger<HrManagerConsumer> logger, ITeamBuildingStrategy strategy,
        IConfiguration configuration, IHttpClientFactory clientFactory) : IHrManagerService
    {
        private readonly ILogger<HrManagerConsumer> _logger = logger;
        private readonly ITeamBuildingStrategy _strategy = strategy;
        private readonly HttpClient _client = clientFactory.CreateClient();

        private readonly int _employeesCount = int.Parse(configuration["EmployeesCount"]!);
        private readonly string _hrDirectorUrl = configuration["HrDirectorUrl"]!;
        private readonly int _httpPostTimeoutMillis = int.Parse(configuration["HttpPostTimeoutMillis"]!);

        private readonly IDictionary<int, HackatonInfo> _hackatonInfos = new Dictionary<int, HackatonInfo>();

        public async Task HandleWishlistMessage(WishlistMessage message)
        {
            _logger.LogInformation("Received preferences from {Type} with ID = {Id}", message.EmployeeType, message.Wishlist.EmployeeId);
            lock (_hackatonInfos)
            {
                if (!_hackatonInfos.TryGetValue(message.HackatonId, out HackatonInfo? value))
                {
                    value = new HackatonInfo([], []);
                    _hackatonInfos[message.HackatonId] = value;
                }
                if (message.EmployeeType == "junior")
                {
                    value.JuniorWishlists.Add(message.Wishlist);
                }
                else
                {
                    value.TeamleadWishlists.Add(message.Wishlist);
                }
                LogRemaining(message.HackatonId);
            }
            var info = _hackatonInfos[message.HackatonId];
            if (info.JuniorWishlists.Count == _employeesCount && info.TeamleadWishlists.Count == _employeesCount)
            {
                await SendTeams(message.HackatonId);
            }
        }

        private void LogRemaining(int hackatonId)
        {
            int remainingJuniors = _employeesCount - _hackatonInfos[hackatonId].JuniorWishlists.Count;
            int remainingTeamleads = _employeesCount - _hackatonInfos[hackatonId].TeamleadWishlists.Count;
            _logger.LogInformation("{Juniors} juniors and {Teamleads} teamleads left for hackaton with id {Id}",
                remainingJuniors, remainingTeamleads, hackatonId);
        }

        private async Task SendTeams(int hackatonId)
        {
            _logger.LogInformation("Hackaton #{Id}: Building teams...", hackatonId);
            var teams = BuildTeams(hackatonId)
                .Select(team => new TeamInfo() { JuniorId = team.Junior.Id, TeamleadId = team.TeamLead.Id });
            _logger.LogInformation("Hackaton #{Id}: Teams have been built successfully!", hackatonId);

            var request = new HrDirectorRequest()
            {
                HackatonId = hackatonId,
                Teams = teams,
                JuniorWishlists = _hackatonInfos[hackatonId].JuniorWishlists,
                TeamleadWishlists = _hackatonInfos[hackatonId].TeamleadWishlists,
            };

            _logger.LogInformation("Hackaton #{Id}: Sending teams info to {Url}", hackatonId, _hrDirectorUrl);
            var client = new TimeoutPostHttpClient(_client, _logger, _hrDirectorUrl, _httpPostTimeoutMillis);
            await client.Send(request);
            _logger.LogInformation("Hackaton #{Id}: Successfully sent teams info to {Url}", hackatonId, _hrDirectorUrl);
        }

        private IEnumerable<Team> BuildTeams(int hackatonId)
        {
            var juniors = _hackatonInfos[hackatonId].JuniorWishlists
                .Select(wishlist => new Employee(wishlist.EmployeeId, ""))
                .ToList();

            var teamleads = _hackatonInfos[hackatonId].TeamleadWishlists
                .Select(wishlist => new Employee(wishlist.EmployeeId, ""))
                .ToList();

            var juniorWishlists = _hackatonInfos[hackatonId].JuniorWishlists;
            var teamleadWishlists = _hackatonInfos[hackatonId].JuniorWishlists;

            return _strategy.BuildTeams(teamleads, juniors, teamleadWishlists, juniorWishlists);
        }
    }
}
