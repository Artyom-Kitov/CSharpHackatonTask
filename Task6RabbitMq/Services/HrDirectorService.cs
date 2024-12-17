using DatabaseEntitiesLib.Entity;
using HackatonTaskLib.Harmony;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Nsu.HackathonProblem.Contracts;
using System.Data;
using Task6RabbitMq.Database;
using Task6RabbitMq.Messages;
using Task6RabbitMq.Requests;

namespace Task6RabbitMq.Services
{
    public interface IHrDirectorService
    {
        void ReceiveFromHrManager(HrDirectorRequest request);
    }

    public class HrDirectorService(ILogger<HrDirectorService> logger, IHarmonyLevelCalculator harmonyCalculator,
        SqliteDatabaseContext context, IConfiguration configuration, IPublishEndpoint publishEndpoint) : IHrDirectorService
    {
        private readonly IHarmonyLevelCalculator _harmonyCalculator = harmonyCalculator;
        private readonly ILogger<HrDirectorService> _logger = logger;
        private readonly SqliteDatabaseContext _context = context;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        private readonly int _hackatonsCount = int.Parse(configuration["HackatonsCount"]!);
        private int _hackatonsHeld;

        public void ReceiveFromHrManager(HrDirectorRequest request)
        {
            _logger.LogInformation("Hackaton #{Id}: Received teams and wishlists from HR manager. Calculating harmony...",
                request.HackatonId);

            var mappedTeams = request.Teams.Select(t =>
                new Nsu.HackathonProblem.Contracts.Team(new Employee(t.TeamleadId, ""), new Employee(t.JuniorId, "")));
            double harmony = _harmonyCalculator.Calculate(mappedTeams, request.JuniorWishlists, request.TeamleadWishlists);

            _logger.LogInformation("Calculated harmony: {Harmony}", harmony);
            SaveHackaton(request.HackatonId, mappedTeams, harmony, request.JuniorWishlists, request.TeamleadWishlists);
            lock (this)
            {
                _hackatonsHeld++;
                if (_hackatonsHeld < _hackatonsCount)
                {
                    StartNextHackaton();
                }
            }
        }

        private void StartNextHackaton()
        {
            var hackaton = new Hackaton() { Harmony = 0.0, Teams = [] };
            _context.Hackatons.Add(hackaton);
            _context.SaveChanges();

            _publishEndpoint.Publish(new HackatonStartedMessage() { HackatonId = hackaton.Id });
        }

        private void SaveHackaton(int hackatonId, IEnumerable<Nsu.HackathonProblem.Contracts.Team> teams, double harmony,
            IEnumerable<Wishlist> juniorWishlists, IEnumerable<Wishlist> teamleadWishlists)
        {
            _logger.LogInformation("Saving hackaton with id = {Id} to database...", hackatonId);
            using var transaction = _context.Database.BeginTransaction(IsolationLevel.RepeatableRead);
            try
            {
                var hackaton = _context.Hackatons.Find(hackatonId)!;
                hackaton.Harmony = harmony;

                SaveTeams(hackaton, teams);
                SaveWishlists(hackaton, juniorWishlists, teamleadWishlists);
                _context.SaveChanges();
                transaction.Commit();

                _logger.LogInformation("Successfully saved hackaton with id = {Id}", hackaton.Id);
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
