using HackatonTaskLib.Harmony;
using HackatonTaskLib.WishlistGeneration;
using Microsoft.EntityFrameworkCore;
using Nsu.HackathonProblem.Contracts;
using Task4Database.Database;
using DatabaseEntitiesLib.Entity;

namespace Task4Database.Service
{
    public class HackatonService(
        IWishlistGenerator wishlistGenerator, 
        ITeamBuildingStrategy strategy,
        IHarmonyLevelCalculator harmonyLevelCalculator,
        DatabaseConfig config)
    {
        private readonly IWishlistGenerator _wishlistGenerator = wishlistGenerator;
        private readonly ITeamBuildingStrategy _strategy = strategy;
        private readonly IHarmonyLevelCalculator _harmonyLevelCalculator = harmonyLevelCalculator;

        private readonly DatabaseConfig _config = config;

        public void GetHackatonInfo(int id, out IEnumerable<Employee> juniors, out IEnumerable<Employee> teamleads,
            out IEnumerable<DatabaseEntitiesLib.Entity.Team> teams, out double harmony)
        {
            using var context = new ApplicationDbContext(_config);
            var hackaton = context.Hackatons
                .Where(h => h.Id == id)
                .Include(h => h.Teams)
                    .ThenInclude(t => t.Junior)
                .Include(h => h.Teams)
                    .ThenInclude(t => t.Teamlead)
                .First();
            var hackatonTeams = hackaton.Teams;
            juniors = hackatonTeams.Select(t => new Employee(t.Junior.Id, t.Junior.Name)).ToList();
            teamleads = hackatonTeams.Select(t => new Employee(t.Teamlead.Id, t.Teamlead.Name)).ToList();
            teams = [.. hackatonTeams];
            harmony = hackaton.Harmony;
        }

        public Hackaton HoldHackaton()
        {
            using var context = new ApplicationDbContext(_config);
            using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
            
            try
            {
                var hackaton = new Hackaton();
                context.Hackatons.Add(hackaton);

                var juniors = context.Juniors
                    .Select(j => new Employee(j.Id, j.Name))
                    .ToList();
                var teamleads = context.Teamleads
                    .Select(t => new Employee(t.Id, t.Name))
                    .ToList();

                _wishlistGenerator.Generate(juniors, teamleads, out var juniorWishlists, out var teamleadWishlists);
                SaveWishlists(context, hackaton, juniorWishlists, teamleadWishlists);

                var teams = _strategy.BuildTeams(teamleads, juniors, teamleadWishlists, juniorWishlists);
                SaveTeams(context, hackaton, teams);

                double harmony = _harmonyLevelCalculator.Calculate(teams, juniorWishlists, teamleadWishlists);
                hackaton.Harmony = harmony;

                context.SaveChanges();
                transaction.Commit();
                return hackaton;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static void SaveTeams(ApplicationDbContext context, Hackaton hackaton, IEnumerable<Nsu.HackathonProblem.Contracts.Team> teams)
        {
            foreach (var team in teams)
            {
                var junior = context.Juniors
                    .Where(j => j.Id == team.Junior.Id)
                    .First();
                var teamlead = context.Teamleads
                    .Where(t => t.Id == team.TeamLead.Id)
                    .First();
                var teamEntity = new DatabaseEntitiesLib.Entity.Team() { Hackaton = hackaton, Junior = junior, Teamlead = teamlead };
                context.Teams.Add(teamEntity);
            }
        }

        private static void SaveWishlists(ApplicationDbContext context, Hackaton hackaton, 
            IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamleadWishlists)
        {
            foreach (var wishlist in juniorWishlists)
            {
                var junior = context.Juniors.Find(wishlist.EmployeeId)
                    ?? throw new InvalidOperationException($"junior with id {wishlist.EmployeeId} does not exist");
                for (int i = 0; i < wishlist.DesiredEmployees.Length; i++)
                {
                    var teamlead = context.Teamleads.Find(wishlist.DesiredEmployees[i])
                        ?? throw new InvalidOperationException($"teamlead with id {wishlist.DesiredEmployees[i]} does not exist");
                    context.JuniorPreferences.Add(
                        new JuniorPreference()
                        { 
                            Hackaton = hackaton, 
                            Junior = junior, 
                            PreferredTeamlead = teamlead,
                            Priority = i
                        });
                }
            }
            foreach (var wishlist in teamleadWishlists)
            {
                var teamlead = context.Teamleads.Find(wishlist.EmployeeId)
                        ?? throw new InvalidOperationException($"teamlead with id {wishlist.EmployeeId} does not exist");
                for (int i = 0; i < wishlist.DesiredEmployees.Length; i++)
                {
                    var junior = context.Juniors
                        .Find(wishlist.DesiredEmployees[i]) 
                        ?? throw new InvalidOperationException($"junior with id {wishlist.DesiredEmployees[i]} does not exist");
                    context.TeamleadPreferences.Add(
                        new TeamleadPreference()
                        {
                            Hackaton = hackaton,
                            Teamlead = teamlead,
                            PreferredJunior = junior,
                            Priority = i
                        });
                }
            }
        }

        public double CalculateAverageHarmony()
        {
            using var context = new ApplicationDbContext(_config);
            return context.Hackatons
                .Select(h => h.Harmony)
                .Average();
        }
    }
}
