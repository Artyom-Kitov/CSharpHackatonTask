using HackatonTaskLib.Harmony;
using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;
using Task1ConsoleApp.Repository;

namespace Task1ConsoleApp.Event
{
    public class Hackaton
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWishlistGenerator _wishListGenerator;
        private readonly ITeamBuildingStrategy _teamBuildingStrategy;
        private readonly IHarmonyLevelCalculator _harmonyLevelCalculator;

        public Hackaton(IEmployeeRepository employeeRepository, IWishlistGenerator wishListGenerator,
            ITeamBuildingStrategy teamBuildingStrategy, IHarmonyLevelCalculator harmonyLevelCalculator)
        {
            _employeeRepository = employeeRepository;
            _wishListGenerator = wishListGenerator;
            _teamBuildingStrategy = teamBuildingStrategy;
            _harmonyLevelCalculator = harmonyLevelCalculator;
        }

        /// <summary>
        /// Hold a hackaton
        /// </summary>
        /// <returns>Average harmony level of teams calculated by harmonyLevelCalculator</returns>
        public double Hold()
        {
            IEnumerable<Employee> juniors = _employeeRepository.GetAllJuniors();
            IEnumerable<Employee> teamLeads = _employeeRepository.GetAllTeamLeads();

            _wishListGenerator.Generate(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);
            IEnumerable<Team> teams = _teamBuildingStrategy.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);

            return _harmonyLevelCalculator.Calculate(teams, juniorWishlists, teamLeadWishlists);
        }
    }
}