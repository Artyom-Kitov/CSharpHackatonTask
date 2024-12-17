using DatabaseEntitiesLib.Entity;
using FluentMigrator.Runner;
using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Microsoft.Extensions.DependencyInjection;
using Task4Database.Database;
using Task4Database.Migrations;
using Task4Database.Service;
using Testcontainers.PostgreSql;

namespace Task3HackatonTests
{
    [TestClass]
    public class DatabaseTest
    {
        private PostgreSqlContainer _container = null!;
        private HackatonService _hackatonService = null!;
        private string ConnectionString = "";

        [TestInitialize]
        public async Task Initialize()
        {
            _container = new PostgreSqlBuilder()
                .WithDatabase("test_hackaton_db")
                .WithUsername("test_user123")
                .WithPassword("test_pass321")
                .Build();

            await _container.StartAsync();

            ConnectionString = _container.GetConnectionString();
            MigrationRunner.RunMigrations(ConnectionString);

            _hackatonService = new HackatonService(
                new RandomWishListGenerator(),
                new GaleShapleyStrategy(),
                new AverageHarmonicCalculator(),
                new DatabaseConfig(ConnectionString)
            );

        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _container.StopAsync();
        }

        [TestMethod]
        public void TestCalculateAverageHarmony_ShouldReturnAverageHarmony()
        {
            // given
            using var context = BuildContext();

            var hackaton1 = new Hackaton() { Id = 1, Harmony = 4.5f };
            var hackaton2 = new Hackaton() { Id = 2, Harmony = 2.7f };
            var hackaton3 = new Hackaton() { Id = 3, Harmony = 1.4f };
            context.Hackatons.AddRange(hackaton1, hackaton2, hackaton3);
            context.SaveChanges();
            double averageHarmony = (hackaton1.Harmony + hackaton2.Harmony + hackaton3.Harmony) / 3;

            // when
            double result = _hackatonService.CalculateAverageHarmony();

            // then
            Assert.AreEqual(averageHarmony, result, 0.0001);
        }

        [TestMethod]
        public void TestGetHackatonInfo_ShouldReturnSavedData()
        {
            // given
            using var context = BuildContext();

            int hackatonId = 1;
            double hackatonHarmony = 1.28;
            var hackaton = new Hackaton() { Id = hackatonId, Harmony = hackatonHarmony, Teams = [] };

            var junior = context.Juniors.Find(1) ?? throw new InvalidOperationException("junior with id = 1 does not exist");
            var teamlead = context.Teamleads.Find(1) ?? throw new InvalidOperationException("teamlead with id = 1 does not exist");
            var team = new Team() { Junior = junior, Teamlead = teamlead, Hackaton = hackaton };
            hackaton.Teams = [team];
            context.Teams.Add(team);
            context.Add(hackaton);
            context.SaveChanges();

            // when
            _hackatonService.GetHackatonInfo(hackatonId, out var juniors, out var teamleads, out var teams, out double harmony);

            // then
            Assert.AreEqual(1, juniors.Count());
            Assert.AreEqual(1, teamleads.Count());
            Assert.AreEqual(1, teams.Count());
            Assert.AreEqual(hackatonHarmony, harmony);
        }

        [TestMethod]
        public void TestHoldHackaton_ShouldSaveDataToDatabase()
        {
            // given
            using var context = BuildContext();
            int hackatonsBefore = context.Hackatons.Count();

            // when
            var held = _hackatonService.HoldHackaton();

            // then
            Assert.AreEqual(hackatonsBefore + 1, context.Hackatons.Count());
            Assert.AreEqual(held.Teams.Count, context.Juniors.Count());
        }

        private ApplicationDbContext BuildContext()
        {
            return new ApplicationDbContext(new DatabaseConfig(ConnectionString));
        }
    }

    internal static class MigrationRunner
    {
        public static void RunMigrations(string connectionString)
        {
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitDatabase).Assembly).For.Migrations())
                .BuildServiceProvider(false);

            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
