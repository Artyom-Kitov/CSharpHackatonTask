using DatabaseEntitiesLib.Entity;
using HackatonTaskLib.Harmony;
using Microsoft.EntityFrameworkCore;
using Nsu.HackathonProblem.Contracts;
using System.Data;
using Task5Http.Database;
using Task5Http.Requests;

namespace Task5Http.Services
{
    public interface IHrDirectorService
    {
        void ReceiveFromHrManager(IEnumerable<TeamInfo> teams, IEnumerable<Wishlist> juniorWishlists,
            IEnumerable<Wishlist> teamleadWishlists);
    }

    public class HrDirectorService(ILogger<HrDirectorService> logger, IHarmonyLevelCalculator harmonyCalculator,
        SqliteDatabaseContext context) : IHrDirectorService
    {
        private readonly IHarmonyLevelCalculator _harmonyCalculator = harmonyCalculator;
        private readonly ILogger<HrDirectorService> _logger = logger;
        private readonly SqliteDatabaseContext _context = context;

        public void ReceiveFromHrManager(IEnumerable<TeamInfo> teams, IEnumerable<Wishlist> juniorWishlists,
            IEnumerable<Wishlist> teamleadWishlists)
        {
            _logger.LogInformation("Received teams and wishlists from HR manager. Calculating harmony...");

            var mappedTeams = teams.Select(t =>
                new Nsu.HackathonProblem.Contracts.Team(new Employee(t.TeamleadId, ""), new Employee(t.JuniorId, "")));
            double harmony = _harmonyCalculator.Calculate(mappedTeams, juniorWishlists, teamleadWishlists);

            _logger.LogInformation("Calculated harmony: {Harmony}", harmony);
            SaveHackaton(mappedTeams, harmony, juniorWishlists, teamleadWishlists);
        }

        private void SaveHackaton(IEnumerable<Nsu.HackathonProblem.Contracts.Team> teams, double harmony,
            IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamleadWishlists)
        {
            _logger.LogInformation("Saving hackaton info to database...");
            using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                var hackaton = new Hackaton() { Harmony = harmony };
                _context.Hackatons.Add(hackaton);

                SaveTeams(hackaton, teams);
                SaveWishlists(hackaton, juniorWishlists, teamleadWishlists);
                _context.SaveChanges();
                transaction.Commit();

                _logger.LogInformation("Successfully saved hackaton info to database, hackaton ID = {Id}", hackaton.Id);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        private void SaveTeams(Hackaton hackaton, IEnumerable<Nsu.HackathonProblem.Contracts.Team> teams)
        {
            foreach (var team in teams)
            {
                var junior = _context.Juniors
                    .Where(j => j.Id == team.Junior.Id)
                    .First();
                var teamlead = _context.Teamleads
                    .Where(t => t.Id == team.TeamLead.Id)
                    .First();
                var teamEntity = new DatabaseEntitiesLib.Entity.Team()
                {
                    Hackaton = hackaton,
                    Junior = junior,
                    Teamlead = teamlead
                };
                _context.Teams.Add(teamEntity);
            }
        }

        private void SaveWishlists(Hackaton hackaton,
    IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamleadWishlists)
        {
            foreach (var wishlist in juniorWishlists)
            {
                var junior = _context.Juniors.Find(wishlist.EmployeeId)
                    ?? throw new InvalidOperationException($"junior with id {wishlist.EmployeeId} does not exist");
                for (int i = 0; i < wishlist.DesiredEmployees.Length; i++)
                {
                    var teamlead = _context.Teamleads.Find(wishlist.DesiredEmployees[i])
                        ?? throw new InvalidOperationException($"teamlead with id {wishlist.DesiredEmployees[i]} does not exist");
                    _context.JuniorPreferences.Add(
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
                var teamlead = _context.Teamleads.Find(wishlist.EmployeeId)
                        ?? throw new InvalidOperationException($"teamlead with id {wishlist.EmployeeId} does not exist");
                for (int i = 0; i < wishlist.DesiredEmployees.Length; i++)
                {
                    var junior = _context.Juniors
                        .Find(wishlist.DesiredEmployees[i])
                        ?? throw new InvalidOperationException($"junior with id {wishlist.DesiredEmployees[i]} does not exist");
                    _context.TeamleadPreferences.Add(
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
    }
}
