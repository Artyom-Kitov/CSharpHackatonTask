using HackatonTaskLib.Harmony;
using HackatonTaskLib.Strategy;
using HackatonTaskLib.WishlistGeneration;
using Nsu.HackathonProblem.Contracts;
using Task2GenericHost;
using Task2GenericHost.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<HackatonWorker>();
        services.AddTransient<IEmployeeRepository, EmployeeCsvRepository>();
        services.AddTransient<ITeamBuildingStrategy, GaleShapleyStrategy>();
        services.AddTransient<IWishlistGenerator, RandomWishListGenerator>();
        services.AddTransient<IHarmonyLevelCalculator, AverageHarmonicCalculator>();
        services.AddTransient<HrManager>();
        services.AddTransient<HrDirector>();
        services.AddTransient<Hackaton>();
    })
    .Build();

await host.RunAsync();