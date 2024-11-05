using FluentMigrator.Runner;
using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;
using Task4Database.Database;
using Task4Database.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        using var serviceProvider = BuildServices();
        using (var scope = serviceProvider.CreateScope())
        {
            MigrateDatabase(scope.ServiceProvider);
        }
        var runner = serviceProvider.GetRequiredService<AppRunner>();
        runner?.Run();
    }

    private static ServiceProvider BuildServices()
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(new DatabaseConfig().ConnectionString)
                .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddDbContext<ApplicationDbContext>()
            .AddTransient<AppRunner>()
            .AddTransient<DatabaseConfig>()
            .AddTransient<HackatonService>()
            .AddTransient<IWishlistGenerator, RandomWishListGenerator>()
            .AddTransient<ITeamBuildingStrategy, GaleShapleyStrategy>()
            .AddTransient<IHarmonyLevelCalculator, AverageHarmonicCalculator>()
            .BuildServiceProvider(true);
    }

    private static void MigrateDatabase(IServiceProvider provider)
    {
        var runner = provider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}