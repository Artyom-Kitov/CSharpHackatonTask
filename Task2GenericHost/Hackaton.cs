using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;
using Task2GenericHost.Repository;

namespace Task2GenericHost
{
    public class Hackaton
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWishlistGenerator _wishlistGenerator;
        private readonly HrManager _hrManager;
        private readonly HrDirector _hrDirector;

        public Hackaton(IEmployeeRepository employeeRepository, IWishlistGenerator wishlistGenerator,
            HrManager hrManager, HrDirector hrDirector)
        {
            _employeeRepository = employeeRepository;
            _wishlistGenerator = wishlistGenerator;
            _hrManager = hrManager;
            _hrDirector = hrDirector;
        }

        /// <summary>
        /// Hold a hackaton
        /// </summary>
        /// <returns>Harmony level</returns>
        public double Hold()
        {
            IEnumerable<Employee> juniors = _employeeRepository.GetAllJuniors();
            IEnumerable<Employee> teamLeads = _employeeRepository.GetAllTeamLeads();

            _wishlistGenerator.Generate(juniors, teamLeads, out var juniorWishlists, out var teamLeadWishlists);

            IEnumerable<Team> teams = _hrManager.BuildTeams(teamLeads, juniors, teamLeadWishlists, juniorWishlists);

            return _hrDirector.CalculateHarmony(teams, juniorWishlists, teamLeadWishlists);
        }
    }
}
