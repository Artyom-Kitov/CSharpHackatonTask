using Nsu.HackathonProblem.Contracts;
using Task5Http.Commons;
using Task5Http.Requests;

namespace Task5Http.Services
{
    public interface IHrManagerService
    {
        void SaveJuniorPreferences(Wishlist wishlist);

        void SaveTeamleadPreferences(Wishlist wishlist);
    }

    public class HrManagerService(ILogger<HrManagerService> logger, IConfiguration configuration,
        IHttpClientFactory clientFactory, ITeamBuildingStrategy strategy) : IHrManagerService
    {
        private readonly ILogger<HrManagerService> _logger = logger;
        private readonly int _employeesCount = int.Parse(configuration["EmployeesCount"]!);

        private readonly HttpClient _client = clientFactory.CreateClient();
        private readonly string _hrDirectorUrl = configuration["HrManager:HrDirectorPostUrl"]!;
        private readonly int _timeoutSeconds = int.Parse(configuration["HrManager:TimeoutSeconds"]!);

        private readonly ITeamBuildingStrategy _strategy = strategy;

        private readonly IDictionary<int, Wishlist> _juniorWishlists = new Dictionary<int, Wishlist>();
        private readonly IDictionary<int, Wishlist> _teamleadWishlists = new Dictionary<int, Wishlist>();

        public void SaveJuniorPreferences(Wishlist wishlist)
        {
            _logger.LogInformation("Received preferences from junior with ID = {Id}", wishlist.EmployeeId);
            lock (_juniorWishlists)
            {
                _juniorWishlists[wishlist.EmployeeId] = wishlist;
            }
            LogRemaining();
            if (_teamleadWishlists.Count == _employeesCount && _juniorWishlists.Count == _employeesCount)
            {
                SendTeams();
            }
        }

        public void SaveTeamleadPreferences(Wishlist wishlist)
        {
            _logger.LogInformation("Received preferences from teamlead with ID = {Id}", wishlist.EmployeeId);
            lock (_teamleadWishlists)
            {
                _teamleadWishlists[wishlist.EmployeeId] = wishlist;
            }
            LogRemaining();
            if (_teamleadWishlists.Count == _employeesCount && _juniorWishlists.Count == _employeesCount)
            {
                SendTeams();
            }
        }

        private void LogRemaining()
        {
            int remainingJuniors = _employeesCount - _juniorWishlists.Count;
            int remainingTeamleads = _employeesCount - _teamleadWishlists.Count;
            _logger.LogInformation("{Juniors} juniors and {Teamleads} teamleads left", remainingJuniors, remainingTeamleads);
        }

        private async void SendTeams()
        {
            _logger.LogInformation("Building teams...");
            var teams = BuildTeams()
                .Select(team => new TeamInfo() { JuniorId = team.Junior.Id, TeamleadId = team.TeamLead.Id });
            _logger.LogInformation("Teams have been built successfully!");

            var request = new HrDirectorRequest()
            {
                Teams = teams,
                JuniorWishlists = _juniorWishlists.Values,
                TeamleadWishlists = _teamleadWishlists.Values
            };

            _logger.LogInformation("Sending teams info to {Url}", _hrDirectorUrl);
            var client = new TimeoutPostHttpClient(_client, _logger, _hrDirectorUrl, 1000 * _timeoutSeconds);
            await client.Send(request);
            _logger.LogInformation("Successfully sent teams info to {Url}", _hrDirectorUrl);
        }

        private IEnumerable<Team> BuildTeams()
        {
            var juniors = _juniorWishlists.Keys
                .Select(key => new Employee(key, ""))
                .ToList();

            var teamleads = _teamleadWishlists.Keys
                .Select(key => new Employee(key, ""))
                .ToList();

            var juniorWishlists = _juniorWishlists.Values;
            var teamleadWishlists = _juniorWishlists.Values;

            return _strategy.BuildTeams(teamleads, juniors, teamleadWishlists, juniorWishlists);
        }
    }
}
